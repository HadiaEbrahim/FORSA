using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models;

public class Skill
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    public ICollection<StudentSkill> StudentSkills { get; set; }
        = new List<StudentSkill>();

    public ICollection<InternshipSkill> InternshipSkills { get; set; }
        = new List<InternshipSkill>();

    public ICollection<SkillRoadmapStep> RoadmapSteps { get; set; }
        = new List<SkillRoadmapStep>();
}