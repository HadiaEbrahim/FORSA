using System.ComponentModel.DataAnnotations;

namespace BLogicLayer.ViewModels;

public class RoadmapStepItemViewModel
{
    public int Id { get; set; }
    public int SkillId { get; set; }
    public int StepOrder { get; set; }

    [Required]
    [StringLength(150)]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    [StringLength(300)]
    public string? ResourceUrl { get; set; }

    public bool IsCompleted { get; set; }
}

public class CreateRoadmapStepViewModel
{
    public int SkillId { get; set; }

    [Required]
    [Range(1, 500)]
    public int StepOrder { get; set; } = 1;

    [Required(ErrorMessage = "Step title is required.")]
    [StringLength(150)]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    [StringLength(300)]
    [Url(ErrorMessage = "Please enter a valid URL.")]
    public string? ResourceUrl { get; set; }
}

public class SkillRoadmapViewModel
{
    public int SkillId { get; set; }
    public string SkillName { get; set; } = string.Empty;
    public int TotalSteps { get; set; }
    public int CompletedSteps { get; set; }
    public int ProgressPercentage => TotalSteps == 0 ? 0 : (int)Math.Round((double)CompletedSteps / TotalSteps * 100);

    public List<RoadmapStepItemViewModel> Steps { get; set; } = new();
    public CreateRoadmapStepViewModel NewStep { get; set; } = new();
}