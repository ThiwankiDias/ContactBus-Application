using ContactBus.Data;
using ContactBus.Models;
using ContactBus.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactBus.Controllers
{
    [Route("api/[controller]")] //<--URL parameter 
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly ContactDbContext contactDbContext;
        //Then we need to inject the instance of the dbcontext class
        public ContactsController(ContactDbContext dbContext) {
            this.contactDbContext = dbContext;
        }


        //Get Endpoint 
        [HttpGet]
        public IActionResult GetAllContacts() //when multiple ActionResults return types are posible we can use IActionResult
        {
            var contacts = contactDbContext.contacts.ToList();
            return Ok(contacts);
        }
        //add the entity to the database ,

        [HttpPost]
        public IActionResult AddContact(AddContactRequestDTO contactRequest)
        {
            var domainModelContact = new Contact
            {
                Id = Guid.NewGuid(),
                Name = contactRequest.Name,
                Email = contactRequest.Email,
                Phone = contactRequest.Phone,
                Favourite = contactRequest.Favourite,
            };
            contactDbContext.contacts.Add(domainModelContact);
            contactDbContext.SaveChanges();
            return Ok(domainModelContact);
        }
        [HttpDelete]
        [Route("{contact_id:guid}")]
        public IActionResult DeleteContact(Guid contact_id)
        {
            var contact = contactDbContext.contacts.Find(contact_id);
            if(contact != null)
            {
                contactDbContext.contacts.Remove(contact);
                contactDbContext.SaveChanges();
            }
            return Ok();
        }
        [HttpPut]
        [Route("{contact_id:guid}")]
        //Either this ApiController attribute is required or the method parameter must be decorated with [FromBody] attribute other wise model binding will
        //not working as expected and the contact data from the angular request will not be mapped to the contactDTO parameter on the UpdateContact method
        public IActionResult UpdateContact(Guid contact_id, [FromBody] AddContactRequestDTO contactDTO )
        {
            var contact =contactDbContext.contacts.Find(contact_id);
            if (contact != null)
            {
                {
                    contact.Name = contactDTO.Name;
                    contact.Email = contactDTO.Email;
                    contact.Phone = contactDTO.Phone;
                    contact.Favourite = contactDTO.Favourite;
                };
                contactDbContext.SaveChanges();
            }
            else
            {
                return NotFound($"{contact_id} not found");
            }
            return Ok(contact);
        }

    }
}
