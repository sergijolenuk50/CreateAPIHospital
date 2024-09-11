using AutoMapper;
using Core.Dtos;
using Core.Exceptions;
using Core.Interfaces;
using Data.Data;
using Data.Entities;
using Data.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class DoctorsService : IDoctorsService
    {
      //  public readonly HospitalDbContext ctx;
        public readonly IMapper mapper;
        private readonly IValidator<CreateDoctorDto> validator;
        private readonly IRepository<Doctor> doctorR;
        public DoctorsService( IMapper mapper, IValidator<CreateDoctorDto> validator, IRepository<Doctor> doctorR)
        {
            //this.ctx = hospitalDbContext;
            this.mapper = mapper;
            this.validator = validator;
            this.doctorR = doctorR;
        }
        public async Task Archive(int id)
        {
            var doctor = await doctorR.GetById(id);
            if (doctor == null) 
                throw new HttpException("Doctors not faund", HttpStatusCode.NotFound); // TODO: exception

            doctor.Archived = true;
            await doctorR.Save();
        }

        public async Task Create(CreateDoctorDto model)
        {
            doctorR.Insert(mapper.Map<Doctor>(model));
            await doctorR.Save();
        }

        public async Task Delete(int id)
        {
            var doctor = await doctorR.GetById(id);
            if (doctor == null) throw new HttpException("Doctor not faund", HttpStatusCode.NotFound); // TODO: exception

            doctorR.Delete(doctor);
            await doctorR.Save();
        }

        public async Task Edit(EditDoctorDto model)
        {
            doctorR.Update(mapper.Map<Doctor>(model));
            await doctorR.Save();

        }

        public async Task<DoctorDto?> Get(int id)
        {
            var doctor = await doctorR.GetById(id);
            if (doctor == null) //return null;
                throw new HttpException("Doctor not faund", HttpStatusCode.NotFound);
            // load related table data
           // await doctorR.(doctor).Reference(x => x.Category).LoadAsync();

            return mapper.Map<DoctorDto>(doctor);
        }

        public async Task<IEnumerable<DoctorDto>> GetAll()
        {
            return mapper.Map<List<DoctorDto>>(await doctorR.GetAll());
        }

        public async Task Restore(int id)
        {
            var doctor = await doctorR.GetById(id);
            if (doctor == null) //return; // TODO: exception
                throw new HttpException("Doctor not faund", HttpStatusCode.NotFound);
            doctor.Archived = false;
            await doctorR.Save();
        }
    }
}
