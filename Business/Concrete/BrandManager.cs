﻿using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class BrandManager : IBrandService
    {

        IBrandDal _brandDal;

        public BrandManager(IBrandDal brandDal)
        {
            _brandDal = brandDal;
        }

        [SecuredOperation("admin")]
        [ValidationAspect(typeof(BrandValidator))]
        [CacheRemoveAspect("IBrandService.Get")]
        public IResult Add(Brand entity)
        {
            _brandDal.Add(entity);
            return new SuccessResult("Brand added succesfully");
            
        }

        [CacheAspect]
        public IDataResult<List<Brand>> GetAll()
        {
            if (_brandDal.GetAll().Count > 0)
            {
                return new SuccessDataResult<List<Brand>>(_brandDal.GetAll());
            }
            else
            {
                return new ErrorDataResult<List<Brand>>("No car found");
            }
        }

        [CacheAspect]
        public IDataResult<Brand> GetById(int id)
        {
            if (_brandDal.Get(b => b.Id == id) != null)                      // check
            {
                return new SuccessDataResult<Brand>(_brandDal.Get(b => b.Id == id));
            }
            else
            {
                return new ErrorDataResult<Brand>("Car with given id is not found");
            }
        }

        [CacheRemoveAspect("IBrandService.Get")]
        public IResult Delete(Brand entity)
        {
            _brandDal.Delete(entity);
            return new SuccessResult("Brand deleted successfully");
        }

        [CacheRemoveAspect("IBrandService.Get")]
        public IResult Update(Brand entity)
        {
            _brandDal.Update(entity);
            return new SuccessResult("Car updated!");
        }
    }
}
