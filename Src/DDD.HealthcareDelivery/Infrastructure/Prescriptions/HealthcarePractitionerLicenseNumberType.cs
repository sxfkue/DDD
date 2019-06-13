﻿using NHibernate;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Core.Infrastructure.Data;
    using Domain.Practitioners;

    internal class HealthcarePractitionerLicenseNumberType<T> : CompositeUserType<T>
        where T : HealthcarePractitionerLicenseNumber
    {

        #region Constructors

        public HealthcarePractitionerLicenseNumberType()
        {
            this.Mutable(false);
            this.Property(p => p.Value, NHibernateUtil.AnsiString);
        }

        #endregion Constructors
    }
}