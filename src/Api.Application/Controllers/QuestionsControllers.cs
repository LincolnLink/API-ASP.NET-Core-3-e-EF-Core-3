
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Domain.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;

namespace Api.Application.Controllers
{

    [EnableCors("Development")]
    [ApiController] //Define que � WebApi
    [Route("api/[controller]")] //Define um roteamento    
    public class QuestionsController : ControllerBase
    {

        private IQuestionsService _service;        

        public QuestionsController(IQuestionsService service)
        {
            this._service = service;
        }

        [EnableCors("Development")]
        [HttpGet]
        [Route("")] // [Authorize("Bearer")        
        public async Task<ActionResult<List<Questions>>> Get() //faz referencia do service
        {
            //parametro removido: [FromServices] IUserService service

            //Verifica se a informa��o que est� vindo da rota � valida!
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); //400 bad request - solicita��o invalida!
            }

            try
            {
                return Ok(await _service.GetAll());
                //return Ok(await service.GetAll());
            }
            catch (ArgumentException e) //trata erros de controller!
            {
                //Resposta para o navegador! - erro 500
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }

        }

        //localhost:5000/api/users/id
        [EnableCors("Development")]
        [HttpGet]
        [Route("{id}", Name = "GetWithId")]
        public async Task<ActionResult<Questions>> Get(Guid id)
        {
            //Verifica se a informa��o que est� vindo da rota � valida!
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); //400 bad request - solicita��o invalida!
            }

            try
            {
                return Ok(await _service.Get(id));
            }
            catch (ArgumentException e) //trata erros de controller!
            {
                //Resposta para o navegador! - erro 500
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }

        }

        [EnableCors("Development")]
        [HttpPost]
        [Route("")]   
        public async Task<ActionResult<Questions>> Post([FromBody] Questions q)
        {
            //Verifica se a informa��o que est� vindo da rota � valida!
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _service.Post(q);
                if (result != null)
                {
                    return Created(new Uri(Url.Link("GetWithId", new { id = result.Id })), result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (ArgumentException e)
            {
                //Resposta para o navegador! - erro 500
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        /*
        [EnableCors("Development")]
        [HttpPut]
        [Route("")]     
        public async Task<ActionResult<Questions>> Put([FromBody] Questions q)
        {
            //Verifica se a informa��o que est� vindo da rota � valida!
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _service.Put(q);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (ArgumentException e)
            {
                //Resposta para o navegador! - erro 500
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }

        }
        */

        [EnableCors("Development")]
        [HttpPut]
        [Route("")]
        public async Task<ActionResult<List<Questions>>> Put([FromBody] Questions[] q)
        {

            //Verifica se a informa��o que est� vindo da rota � valida!
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _service.PutAll(q);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (ArgumentException e)
            {
                //Resposta para o navegador! - erro 500
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }

        }

        [EnableCors("Development")]
        [HttpDelete("{id}")] //("{id}")
        [Route("{id}")]  
        public async Task<ActionResult> Delete(Guid id)
        {
            //Verifica se a informa��o que est� vindo da rota � valida!
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _service.Delete(id);
                if (result)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (ArgumentException e)
            {
                //Resposta para o navegador! - erro 500
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }

        }
    }
}