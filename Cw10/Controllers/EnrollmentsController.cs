﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cw10.Models;
using System.Data.SqlClient;
using Cw10.DTO.Requests;
using Cw10.DTO.Responses;
using Cw10.Services;
using Microsoft.AspNetCore.Authorization;

namespace Cw10.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase
    {
        private IStudentDbService _service;
        public EnrollmentsController(IStudentDbService service)
        {
            _service = service;
        }
        [HttpPost]
        //[Authorize(Roles = "empoloyee")]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {

            try
            {
                return Ok(_service.EnrollStudent(request));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        [HttpPost]
        [Route("promotions")]
        //[Authorize(Roles = "empoloyee")]
        public IActionResult PromoteStudents(PromoteStudentsRequest request)
        {
            PromoteStudentsResponse resp = _service.PromoteStudents(request);
            if (resp == null)
            {
                return BadRequest("404 Not Found");
            }
            else
            {
                return Ok(resp);
            }
        }
    }
}

