using CPP_Lab13API.Models;
using CPP_Lab13API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Components.Forms;
using ClassLibraryLab5;

namespace App.Controllers;

[ApiController]
[Route("api/labs")]
public class LabsController : ControllerBase
{
    [HttpPost("lab1")]
    [Authorize]
    public ActionResult Lab1([FromBody] Lab requestModel)
    {
        try
        {
            List<string> resultList = ClassLab1.Execute(requestModel.inputString);
            string result = "";
            foreach (string element in resultList)
            {
                result += element + "\r\n";
            }

            return Ok(new { result });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost("lab2")]
    [Authorize]
    public ActionResult Lab2([FromBody] Lab requestModel)
    {
        try
        {
            string result = ClassLab2.Execute(requestModel.inputString).ToString();
            return Ok(new { result });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost("lab3")]
    [Authorize]
    public ActionResult Lab3([FromBody] Lab requestModel)
    {
        try
        {
            string result = ClassLab3.Execute(requestModel.inputString);
            return Ok(new { result });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}