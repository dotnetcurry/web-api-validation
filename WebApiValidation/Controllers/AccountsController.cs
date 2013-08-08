using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApiValidation.Filters;
using WebApiValidation.Models;

namespace WebApiValidation.Controllers
{
    public class AccountsController : ApiController
    {
        private WebApiValidationContext db = new WebApiValidationContext();

        // GET api/Accounts
        public IEnumerable<Account> GetAccounts()
        {
            return db.Accounts.AsEnumerable();
        }

        // GET api/Accounts/5
        public Account GetAccount(int id)
        {
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return account;
        }

        // PUT api/Accounts/5
        [ValidateModelFilter()]
        public HttpResponseMessage PutAccount(int id, Account account)
        {
            //if (!ModelState.IsValid)
            //{
            //    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            //}

            if (id != account.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(account).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/Accounts
        public HttpResponseMessage PostAccount(Account account)
        {
            if (ModelState.IsValid)
            {
                db.Accounts.Add(account);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, account);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = account.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Accounts/5
        public HttpResponseMessage DeleteAccount(int id)
        {
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Accounts.Remove(account);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, account);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}