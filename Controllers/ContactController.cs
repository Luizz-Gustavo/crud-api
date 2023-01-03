using Microsoft.AspNetCore.Mvc;
using MODULOAPI.Entities;
using MODULOAPI.Context;
using Microsoft.EntityFrameworkCore;

namespace MODULOAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly ContactBookContext _context;

        public ContactController(ContactBookContext context)
        {
            _context = context;
        }

        //This method creates a contact in the database!
        [HttpPost("CreateContact")]
        public async Task<IActionResult> createContact(Contact contact)
        {

            if (!IsValidContact(contact))
            {
                return BadRequest();
            }

            _context.Add(contact);
            await _context.SaveChangesAsync();
            return Ok(contact);
        }

        //this method checks if the contact is valid.
        private bool IsValidContact(Contact contact)
        {
            if (string.IsNullOrEmpty(contact.Name))
            {
                throw new ArgumentException("Name is required");
            }

            if (!IsValidPhoneNumber(contact.Telephone))
            {
                throw new ArgumentException("Phone number must contain only digits.");
            }

            return true;
        }

        //This method checks if a string is a valid phone number.
        private bool IsValidPhoneNumber(string phoneNumber)
        {
            if (!phoneNumber.All(char.IsDigit))
            {
                throw new ArgumentException("Phone number must contain only digits.");
            }

            return true;
        }

        //This method retrieves a contact by id!
        [HttpGet("GetContactById")]
        public async Task<IActionResult> getContactById(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
            {
                return NotFound();
            }

            return Ok(contact);
        }

        //This method retrieve a contact by its name!
        [HttpGet("GetContactByName")]
        public async Task<IActionResult> getContactByName(string nameContact)
        {
            var contact = await _context.Contacts.Where(x => x.Name.Contains(nameContact)).ToListAsync();

            if (nameContact == null || contact.Count() == 0)
            {
                return NotFound();
            }

            return Ok(contact);
        }

        //This method updates a contact in the database!
        [HttpPatch("UpdateContact")]
        public async Task<IActionResult> updateContact(int id, Contact contact)
        {
            var registeredContact = await _context.Contacts.FindAsync(id);
            if (registeredContact == null)
            {
                return NotFound();
            }

            if (!IsValidPhoneNumber(contact.Telephone))
            {
                throw new ArgumentException("Phone number must contain only digits.");
            }

            registeredContact.Name = contact.Name;
            registeredContact.Telephone = contact.Telephone;
            registeredContact.Active = contact.Active;

            _context.Contacts.Update(registeredContact);
            await _context.SaveChangesAsync();

            return Ok(registeredContact);
        }

        //This method deletes a contact from the database!
        [HttpDelete("DeleteContact")]
        public async Task<IActionResult> deleteContact(int id)
        {
            var registeredContact = await _context.Contacts.FindAsync(id);

            if (registeredContact == null)
            {
                return NotFound();
            }

            _context.Contacts.Remove(registeredContact);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}