using BLogicLayer.ViewModels;

namespace BLogicLayer.Interfaces;

public interface IRoadmapService
{
    Task<SkillRoadmapViewModel?> GetSkillRoadmapAsync(int skillId, string? userEmail);
    Task<bool> AddStepAsync(CreateRoadmapStepViewModel model);
    Task<bool> DeleteStepAsync(int stepId);
    Task<bool> ToggleStepCompletionAsync(int stepId, string userEmail);
}