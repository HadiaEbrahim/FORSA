using BLogicLayer.Interfaces;
using BLogicLayer.ViewModels;
using DataAccessLayer.Data;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BLogicLayer.Services;

public class RoadmapService : IRoadmapService
{
    private readonly ApplicationDbContext _context;

    public RoadmapService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SkillRoadmapViewModel?> GetSkillRoadmapAsync(int skillId, string? userEmail)
    {
        var skill = await _context.Skills
            .Include(s => s.RoadmapSteps)
            .FirstOrDefaultAsync(s => s.Id == skillId);

        if (skill == null)
            return null;

        int? studentId = null;
        if (!string.IsNullOrWhiteSpace(userEmail))
        {
            studentId = await _context.Students
                .Where(st => st.Email.ToLower() == userEmail.ToLower())
                .Select(st => (int?)st.Id)
                .FirstOrDefaultAsync();
        }

        var completedStepIds = new HashSet<int>();
        if (studentId.HasValue)
        {
            var stepIds = skill.RoadmapSteps.Select(r => r.Id).ToList();
            var ids = await _context.StudentRoadmapProgresses
                .Where(p => p.StudentId == studentId.Value &&
                            stepIds.Contains(p.SkillRoadmapStepId) &&
                            p.IsCompleted)
                .Select(p => p.SkillRoadmapStepId)
                .ToListAsync();

            completedStepIds = ids.ToHashSet();
        }

        var orderedSteps = skill.RoadmapSteps
            .OrderBy(r => r.StepOrder)
            .ThenBy(r => r.Id)
            .Select(r => new RoadmapStepItemViewModel
            {
                Id = r.Id,
                SkillId = r.SkillId,
                StepOrder = r.StepOrder,
                Title = r.Title,
                Description = r.Description,
                ResourceUrl = r.ResourceUrl,
                IsCompleted = completedStepIds.Contains(r.Id)
            })
            .ToList();

        var nextOrder = orderedSteps.Any() ? orderedSteps.Max(s => s.StepOrder) + 1 : 1;

        return new SkillRoadmapViewModel
        {
            SkillId = skill.Id,
            SkillName = skill.Name,
            TotalSteps = orderedSteps.Count,
            CompletedSteps = orderedSteps.Count(s => s.IsCompleted),
            Steps = orderedSteps,
            NewStep = new CreateRoadmapStepViewModel
            {
                SkillId = skill.Id,
                StepOrder = nextOrder
            }
        };
    }

    public async Task<bool> AddStepAsync(CreateRoadmapStepViewModel model)
    {
        var skillExists = await _context.Skills.AnyAsync(s => s.Id == model.SkillId);
        if (!skillExists)
            return false;

        var step = new SkillRoadmapStep
        {
            SkillId = model.SkillId,
            StepOrder = model.StepOrder,
            Title = model.Title.Trim(),
            Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim(),
            ResourceUrl = string.IsNullOrWhiteSpace(model.ResourceUrl) ? null : model.ResourceUrl.Trim()
        };

        _context.SkillRoadmapSteps.Add(step);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteStepAsync(int stepId)
    {
        var step = await _context.SkillRoadmapSteps.FindAsync(stepId);
        if (step == null)
            return false;

        _context.SkillRoadmapSteps.Remove(step);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleStepCompletionAsync(int stepId, string userEmail)
    {
        if (string.IsNullOrWhiteSpace(userEmail))
            return false;

        var studentId = await _context.Students
            .Where(st => st.Email.ToLower() == userEmail.ToLower())
            .Select(st => (int?)st.Id)
            .FirstOrDefaultAsync();

        if (!studentId.HasValue)
            return false;

        var stepExists = await _context.SkillRoadmapSteps.AnyAsync(s => s.Id == stepId);
        if (!stepExists)
            return false;

        var progress = await _context.StudentRoadmapProgresses
            .FirstOrDefaultAsync(p => p.StudentId == studentId.Value &&
                                      p.SkillRoadmapStepId == stepId);

        if (progress == null)
        {
            progress = new StudentRoadmapProgress
            {
                StudentId = studentId.Value,
                SkillRoadmapStepId = stepId,
                IsCompleted = true,
                CompletedAt = DateTime.UtcNow
            };
            _context.StudentRoadmapProgresses.Add(progress);
        }
        else
        {
            progress.IsCompleted = !progress.IsCompleted;
            progress.CompletedAt = progress.IsCompleted ? DateTime.UtcNow : null;
        }

        await _context.SaveChangesAsync();
        return true;
    }
}