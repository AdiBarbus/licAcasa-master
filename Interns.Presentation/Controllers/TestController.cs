using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Interns.Core.Data;
using Interns.Services.IService;
using log4net;
using Interns.Services.Models;


namespace Interns.Presentation.Controllers
{
    public class TestController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(TestController));
        private readonly ITestService testService;
        private readonly IQuestionService questionService;

        public TestController(ITestService service, IQuestionService questionServ)
        {
            testService = service;
            questionService = questionServ;
        }

        [HttpGet]
        public ActionResult Tests()
        {
            IEnumerable<Test> qas = new List<Test>();

            Log.Debug("Started getting all the Tests");
            try
            {
                qas = testService.GetTests();
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }

            return View(qas);
        }

        [HttpGet]
        [Route("test/TakeTest/{testId}")]
        public ActionResult TakeTest(int testId)
        {
            QAModelView2 model = new QAModelView2();
            Log.Debug($"Getting the adverise with the domain id:{testId}");

            try
            {
                model.Questions = testService.GetQuestionsByTests(testId).ToList();
                //model.Answers = answerService.GetAnswers().ToList();
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult TakeTests()
        {
            var questions = questionService.GetQuestions();
            int points = 0;

            foreach (var que in questions)
            {

                string test = Request.Form[$"checkedAns-{que.Id}"];
                if (!String.IsNullOrEmpty(test))
                {
                    int testValue = Int32.Parse(test);
                    foreach (var queAnswer in que.Answers)
                    {
                        if (queAnswer.Id == testValue && queAnswer.IsCorrect)
                        {
                            points++;
                        }
                    }
                }
                
            }

            return RedirectToAction("Results", new {id = points});
        }

        [HttpGet]
        public ActionResult Results(int id)
        {
            return View(id);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateTest()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateTest(Test test)
        {
            Log.Debug("Creating new Test");

            try
            {
                testService.InsertTest(test);
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }

            return RedirectToAction("Tests");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult EditTest(int id)
        {
            var editTest = testService.GetTest(id);
            return View(editTest);
        }

        [HttpPost]
        public ActionResult EditTest(Test test)
        {
            Log.Debug($"Updating {test.Name}");
            if (ModelState.IsValid)
            {
                try
                {
                    testService.UpdateTest(test);
                    Log.Info($"{test.Name} was updated succesfully");
                }
                catch (Exception e)
                {
                    Log.Error(e.ToString());
                }
            }

            return RedirectToAction("Tests");
        }

        [HttpGet]
        public ActionResult DeleteTest(Test test)
        {
            Log.Debug($"Deleting {test.Name}");

            try
            {
                testService.DeleteTest(test);
                Log.Info($"{test.Name} was deleted succesfully");
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
            return RedirectToAction("Tests");
        }

    }
}