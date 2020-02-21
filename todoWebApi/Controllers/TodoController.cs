using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using todoWebApi.Data;

namespace todoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : Controller {
        private readonly TodoContext _context;

        public TodoController(TodoContext context) {
            _context = context;
        }

        // GetAll() is automatically recognized as
        // http://localhost:<port #>/api/todo
        [HttpGet]
        public IEnumerable<ToDo> GetAll() {
            return _context.ToDos.ToList();
        }

        // GetById() is automatically recognized as
        // http://localhost:<port #>/api/todo/{id}

        // For example:
        // http://localhost:<port #>/api/todo/1

        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(long id) {
            var item = _context.ToDos.FirstOrDefault(t => t.Id == id);
            if (item == null) {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody]ToDo todo) {
            if (todo.Description == null || todo.Description == "") {
                return BadRequest();
            }
            _context.ToDos.Add(todo);
            _context.SaveChanges();
            return new ObjectResult(todo);
        }

        [HttpPut]
        [Route("MyEdit")] // Custom route
        public IActionResult GetByParams([FromBody]ToDo todo) {
            var item = _context.ToDos.Where(t => t.Id == todo.Id).FirstOrDefault();
            if (item == null) {
                return NotFound();
            }
            else {
                item.IsComplete  = todo.IsComplete;
                _context.SaveChanges();
            }
            return new ObjectResult(item);
        }

        [HttpDelete]
        [Route("MyDelete")] // Custom route
        public IActionResult MyDelete(long Id)
        {
            var item = _context.ToDos.Where(t => t.Id == Id).FirstOrDefault();
            if (item == null)
            {
                return NotFound();
            }
            _context.ToDos.Remove(item);
            _context.SaveChanges();
            return new ObjectResult(item);
        }

        [HttpGet]
        // Distinguish the method so controller can find it if more than
        // one POST method exists.
        [Route("EmailHelper")] 
        public JsonResult EmailHelper(string email, string message) {
            dynamic messageArray  = new JArray();
            dynamic responseObj   = new JObject();
            responseObj.Message   = "Hi " + email + ". " + message;
            responseObj.Status    = 200;
            messageArray.Add(responseObj);
            return Json(messageArray);
        }

    }
}