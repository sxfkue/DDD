﻿using Xunit;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Principal;
using System;
using FluentAssertions;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Core.Domain;
    using Domain.Prescriptions;
    using Core.Infrastructure.Testing;
    using Infrastructure;
    using Infrastructure.Prescriptions;
    using Mapping;

    public abstract class PharmaceuticalPrescriptionRevokerTests<TFixture> : IDisposable
        where TFixture : IDbFixture<IHealthcareConnectionFactory>
    {

        #region Fields

        private HealthcareContext context;

        #endregion Fields

        #region Constructors

        protected PharmaceuticalPrescriptionRevokerTests(TFixture fixture)
        {
            this.Fixture = fixture;
            this.Repository = this.CreateRepository();
            this.Handler = new PharmaceuticalPrescriptionRevoker
            (
                Repository,
                new EventPublisher()
            );
        }

        #endregion Constructors

        #region Properties

        protected TFixture Fixture { get; }
        protected PharmaceuticalPrescriptionRevoker Handler { get; }
        protected IAsyncRepository<PharmaceuticalPrescription> Repository { get; }

        #endregion Properties

        #region Methods

        public void Dispose()
        {
            this.context.Dispose();
        }

        [Fact]
        public async Task HandleAsync_WhenCalled_RevokePharmaceuticalPrescription()
        {
            // Arrange
            this.Fixture.ExecuteScriptFromResources("RevokePharmaceuticalPrescription");
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("d.duck"), new string[] { "User" });
            var command = CreateCommand();
            // Act
            await this.Handler.HandleAsync(command);
            // Assert
            var prescription = await this.Repository.FindAsync(new PrescriptionIdentifier(command.PrescriptionIdentifier));
            prescription.Status.Should().Be(Domain.Prescriptions.PrescriptionStatus.Revoked);
        }


        protected abstract HealthcareContext CreateContext();

        protected abstract IObjectTranslator<IEvent, EventState> CreateEventTranslator();

        private static RevokePharmaceuticalPrescription CreateCommand()
        {
            return new RevokePharmaceuticalPrescription
            {
                PrescriptionIdentifier = 1,
                RevocationReason = "Erreur"
            };
        }

        private IAsyncRepository<PharmaceuticalPrescription> CreateRepository()
        {
            this.context = this.CreateContext();
            return new PharmaceuticalPrescriptionRepository
            (
                this.context,
                new Domain.Prescriptions.BelgianPharmaceuticalPrescriptionTranslator(),
                this.CreateEventTranslator()
            );
        }

        #endregion Methods

    }
}
