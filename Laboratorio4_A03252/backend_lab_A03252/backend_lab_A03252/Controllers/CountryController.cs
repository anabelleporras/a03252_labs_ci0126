﻿using backend_lab_A03252.Models;
using backend_lab_A03252.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend_lab_A03252.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly CountryService countryService;

        public CountryController()
        {
            countryService = new CountryService();
        }

        [HttpGet]
        public List<CountryModel> Get()
        {
            var countries = countryService.GetCountries();
            return countries;
        }
    }
}
