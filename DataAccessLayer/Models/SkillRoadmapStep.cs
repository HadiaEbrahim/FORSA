using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models;

public class SkillRoadmapStep
{
    public int Id { get; set; }

    public int SkillId { get; set; }
    public Skill Skill { get; set; }

    [Required]
    public int StepOrder { get; set; }

    [Required]
    [StringLength(150)]
    public string Title { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }

    [StringLength(300)]
    public string? ResourceUrl { get; set; }

    public ICollection<StudentRoadmapProgress> StudentProgresses { get; set; }
        = new List<StudentRoadmapProgress>();
}