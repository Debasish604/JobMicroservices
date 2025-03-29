using AutoMapper;
using JobServices.data;
using JobsServices.models.Dto;
using JobsServices.models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace JobServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly AppDBContext _db;
        private readonly IMapper _mapper;
        private ResponseDto _response;

        public JobsController(AppDBContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        [HttpGet("{hiringManagerId}")]
        public IActionResult GetJobsByHiringManager(string hiringManagerId)
        {
            try
            {
                var jobs = _db.Jobs
                    .Where(job => job.HiringManagerId == hiringManagerId)
                    .ToList();

                if (jobs == null || jobs.Count == 0)
                {
                    _response.IsSuccess = false;
                    _response.Message = "No jobs found for the specified HiringManagerId.";
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<List<JobDto>>(jobs);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return Ok(_response);
        }

        [HttpPost]
        [Route("InsertJob")]
        public IActionResult InsertJob([FromBody] CreateJobDto jobDto)
        {
            try
            {
                if (jobDto == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Invalid job details provided.";
                    return BadRequest(_response);
                }

                // Map CreateJobDto to Job entity
                var jobEntity = _mapper.Map<Job>(jobDto);

                // Set server-side values
                jobEntity.CreatedOn = DateTime.UtcNow; // Use UTC time here

                // Add job entity to the database
                _db.Jobs.Add(jobEntity);
                _db.SaveChanges();

                // Map the saved entity back to JobDto for the response
                var resultDto = _mapper.Map<JobDto>(jobEntity);

                _response.IsSuccess = true;
                _response.Message = "Job successfully created.";
                _response.Result = resultDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"An error occurred: {ex.Message}";

                // Log the inner exception for debugging
                if (ex.InnerException != null)
                {
                    _response.Message += $" Inner Exception: {ex.InnerException.Message}";
                }

                return StatusCode(500, _response);
            }

            return Ok(_response);
        }



    }
}
