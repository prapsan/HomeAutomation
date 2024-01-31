using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using WebApplication1.Devices;
using WebApplication1.Helpers;
using WebApplication1.Interfaces;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly IRepository<Device> _devicesDatabase;
        private readonly IMemoryCache _memoryCache;
        private const string UndoStackCacheKey = "CachedUndoStack";

        public DevicesController(IRepository<Device> devicesDatabase, IMemoryCache memoryCache)
        {
            _devicesDatabase = devicesDatabase;
            _memoryCache = memoryCache;
            InitializeStack();
        }

        private void InitializeStack()
        {
            if (!_memoryCache.TryGetValue(UndoStackCacheKey, out Stack<UndoableAction> undoStack))
            {
                undoStack = new Stack<UndoableAction>();
                _memoryCache.Set(UndoStackCacheKey, undoStack);
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<IDevice>> GetAllDevices()
        {
            var devices = _devicesDatabase.GetAll();
            return Ok(new { message = "Success", data = devices });
        }

        [HttpGet("{id}")]
        public ActionResult<Device> GetDevice(Guid id)
        {
            var device = _devicesDatabase.GetById(id);

            if (device == null)
            {
                return NotFound(new { message = "Device not found", data = new { } });
            }

            return Ok(device);
        }

        [HttpPost]
        public ActionResult<Device> RegisterDevice([FromBody] Device device)
        {
            if (device == null)
            {
                return BadRequest("Device data is invalid");
            }

            _devicesDatabase.Add(device);

            if (_memoryCache.TryGetValue(UndoStackCacheKey, out Stack<UndoableAction> undoStack))
            {
                undoStack.Push(new UndoableAction
                {
                    Type = ActionType.RemoveDevice,
                    Data = device.Id
                });
            }

            return CreatedAtAction(nameof(GetDevice), new { id = device.Id }, new { message = "Device registered", data = device });
        }

        [HttpPost("{id}")]
        public ActionResult<Device> Trigger(Guid id)
        {
            var device = _devicesDatabase.GetById(id);

            if (device == null)
            {
                return NotFound(new { message = "Device not found", data = new { } });
            }

            device.Trigger();

            return Ok(new { message = "Success", data = device.Status() });
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            var device = _devicesDatabase.GetById(id);

            if (device == null)
            {
                return NotFound(new { message = "Device not found", data = new { } });
            }

            _devicesDatabase.Delete(id);

            if (_memoryCache.TryGetValue(UndoStackCacheKey, out Stack<UndoableAction> undoStack))
            {
                undoStack.Push(new UndoableAction
                {
                    Type = ActionType.AddDevice,
                    Data = device
                });
            }

            return Ok(new { message = "Device deleted", data = device });
        }

        [HttpPost("undo")]
        public ActionResult UndoLastAction()
        {
            if (_memoryCache.TryGetValue(UndoStackCacheKey, out Stack<UndoableAction> undoStack) && undoStack.Any())
            {
                var lastAction = undoStack.Pop();

                switch (lastAction.Type)
                {
                    case ActionType.AddDevice:
                        _devicesDatabase.Add((Device)lastAction.Data);
                        break;
                    case ActionType.RemoveDevice:
                        _devicesDatabase.Delete((Guid)lastAction.Data);
                        break;
                }

                return Ok(new { message = "Undo successful", data = lastAction });
            }

            return NotFound(new { message = "No actions to undo", data = new { } });
        }
    }

    public class UndoableAction
    {
        public ActionType Type { get; set; }
        public object Data { get; set; }
    }

    public class HomeAutomationUndoManager
    {
        private readonly Stack<UndoableAction> _undoStack = new Stack<UndoableAction>();

        public void AddUndoableAction(UndoableAction action)
        {
            _undoStack.Push(action);
        }

        public UndoableAction UndoLastAction()
        {
            if (_undoStack.Any())
            {
                return _undoStack.Pop();
            }

            return null;
        }
    }
}
