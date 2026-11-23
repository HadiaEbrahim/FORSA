using BLogicLayer.Interfaces;
using BLogicLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mawjood_Internship.Controllers
{
    [Authorize]
    public class SkillController : Controller
    {
        private readonly ISkillService _skillService;
        private readonly IRoadmapService _roadmapService;

        public SkillController(
            ISkillService skillService,
            IRoadmapService roadmapService)
        {
            _skillService = skillService;
            _roadmapService = roadmapService;
        }

        // GET: Skill
        public async Task<IActionResult> Index()
        {
            var skills = await _skillService.GetAll();
            return View(skills);
        }

        // GET: Skill/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var skill = await _skillService.GetById(id);
            if (skill == null)
                return NotFound();

            return View(skill);
        }

        // GET: Skill/Roadmap/5
        [HttpGet]
        public async Task<IActionResult> Roadmap(int id)
        {
            var vm = await _roadmapService.GetSkillRoadmapAsync(id, User.Identity?.Name);
            if (vm == null)
                return NotFound();

            return View(vm);
        }

        // POST: Skill/AddRoadmapStep
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRoadmapStep(CreateRoadmapStepViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please check step fields (Title is required and Resource URL must be valid).";
                return RedirectToAction(nameof(Roadmap), new { id = model.SkillId });
            }

            var added = await _roadmapService.AddStepAsync(model);
            if (!added)
                return NotFound();

            return RedirectToAction(nameof(Roadmap), new { id = model.SkillId });
        }

        // POST: Skill/DeleteRoadmapStep
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRoadmapStep(int stepId, int skillId)
        {
            await _roadmapService.DeleteStepAsync(stepId);
            return RedirectToAction(nameof(Roadmap), new { id = skillId });
        }

        // POST: Skill/ToggleStepCompletion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStepCompletion(int stepId, int skillId)
        {
            var email = User.Identity?.Name;
            if (!string.IsNullOrWhiteSpace(email))
            {
                await _roadmapService.ToggleStepCompletionAsync(stepId, email);
            }

            return RedirectToAction(nameof(Roadmap), new { id = skillId });
        }

        // GET: Skill/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Skill/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(SkillViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _skillService.Add(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: Skill/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var skill = await _skillService.GetById(id);
            if (skill == null)
                return NotFound();

            return View(skill);
        }

        // POST: Skill/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, SkillViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            await _skillService.Update(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: Skill/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var skill = await _skillService.GetById(id);
            if (skill == null)
                return NotFound();

            return View(skill);
        }

        // POST: Skill/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _skillService.Delete(id);
            if (!deleted)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}