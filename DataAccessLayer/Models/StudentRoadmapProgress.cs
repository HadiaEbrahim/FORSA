using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models;

public class StudentRoadmapProgress
{
    public int Id { get; set; }

    public int StudentId { get; set; }
    public Student Student { get; set; }

    public int SkillRoadmapStepId { get; set; }
    public SkillRoadmapStep SkillRoadmapStep { get; set; }

    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
}
