﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.OData.Results;
using Moq;
using NUnit.Framework;
using PipeTally.Controllers;
using PipeTally.DataModel;
using PipeTally.Services;
using PipeTally.Services.Abstract;

namespace PipeTally.TEST.Controllers
{
    [TestFixture]
    public class MeasurementControllerTests
    {
        [OneTimeSetUp]
        public void SetUp()
        {
        }

        [Test]
        public void DeleteRecordNotFound()
        {
            //Setup
            var measurementServiceMock = new Mock<IMeasurementService>();
            measurementServiceMock.Setup(x => x.Delete(1)).Returns(false); //this will trigger not found
            var dataModelMock = new Mock<IDataModel>();

            //Execute
            var target = new MeasurementController(measurementServiceMock.Object, dataModelMock.Object);
            var results = target.Delete(1);

            //Evalute
            Assert.IsInstanceOf<NotFoundResult>(results);
        }

        [Test]
        public void DeleteSuccess()
        {
            //Setup
            var measurementServiceMock = new Mock<IMeasurementService>();
            measurementServiceMock.Setup(x => x.Delete(1)).Returns(true);
            var dataModelMock = new Mock<IDataModel>();

            //Execute
            var target = new MeasurementController(measurementServiceMock.Object, dataModelMock.Object);
            var results = target.Delete(1);

            //Evalute
            dataModelMock.Verify(x => x.SaveChanges(), Times.Once);
            Assert.IsInstanceOf<StatusCodeResult>(results);
            Assert.AreEqual(HttpStatusCode.NoContent, ((StatusCodeResult)results).StatusCode);
        }

        [Test]
        public void DeleteExceptionThrown()
        {
            //Setup
            var measurementServiceMock = new Mock<IMeasurementService>();
            measurementServiceMock.Setup(x => x.Delete(1)).Throws<Exception>();
            var dataModelMock = new Mock<IDataModel>();
            //Execute
            var target = new MeasurementController(measurementServiceMock.Object, dataModelMock.Object);
            var results = target.Delete(1);
            //Evaluate
            Assert.IsInstanceOf<ExceptionResult>(results);
        }

        [Test]
        public void PatchRecordNotFound()
        {
            //Setup
            var measurementServiceMock = new Mock<IMeasurementService>();
            measurementServiceMock.Setup(x => x.Update(It.IsAny<Measurement>())).Returns(false); //this will trigger not found
            var dataModelMock = new Mock<IDataModel>();

            //Execute
            var target = new MeasurementController(measurementServiceMock.Object, dataModelMock.Object);
            var results = target.Patch(new Measurement());

            //Evalute
            Assert.IsInstanceOf<NotFoundResult>(results);
        }

        [Test]
        public void PatchSuccess()
        {
            //Setup
            var measurementServiceMock = new Mock<IMeasurementService>();
            measurementServiceMock.Setup(x => x.Update(It.IsAny<Measurement>())).Returns(true);
            var dataModelMock = new Mock<IDataModel>();

            //Execute
            var target = new MeasurementController(measurementServiceMock.Object, dataModelMock.Object);
            var results = target.Patch(new Measurement());

            //Evalute
            dataModelMock.Verify(x => x.SaveChanges(), Times.Once);
            Assert.IsInstanceOf<UpdatedODataResult<Measurement>>(results);
        }

        [Test]
        public void PatchExceptionThrown()
        {
            //Setup
            var measurementServiceMock = new Mock<IMeasurementService>();
            measurementServiceMock.Setup(x => x.Update(It.IsAny<Measurement>())).Throws<Exception>();
            var dataModelMock = new Mock<IDataModel>();
            //Execute
            var target = new MeasurementController(measurementServiceMock.Object, dataModelMock.Object);
            var results = target.Patch(new Measurement());
            //Evaluate
            Assert.IsInstanceOf<ExceptionResult>(results);
        }

        [Test]
        public void GetSuccess()
        {
            //Setup
            var measurementServiceMock = new Mock<IMeasurementService>();
            var measurementServiceList = new List<Measurement>
            {
                new Measurement
                {
                    MeasurementId = 1 //What the query is looking for.
                }
            };
            IQueryable<Measurement> measurementQueryable = measurementServiceList.AsQueryable();
            measurementServiceMock.Setup(x => x.Read()).Returns(measurementQueryable);
            var dataModelMock = new Mock<IDataModel>();

            //Execute
            var target = new MeasurementController(measurementServiceMock.Object, dataModelMock.Object);
            var results = target.Get();

            //Evalute
            Assert.IsInstanceOf<IQueryable<Measurement>>(results);
        }

        [Test]
        public void GetSuccess_WithRecord()
        {
            //Setup
            var measurementServiceMock = new Mock<IMeasurementService>();
            var measurementServiceList = new List<Measurement>
            {
                new Measurement
                {
                    MeasurementId = 1 //What the query is looking for.
                }
            };
            IQueryable<Measurement> measurementQueryable = measurementServiceList.AsQueryable();
            measurementServiceMock.Setup(x => x.Read()).Returns(measurementQueryable);
            var dataModelMock = new Mock<IDataModel>();

            //Execute
            var target = new MeasurementController(measurementServiceMock.Object, dataModelMock.Object);
            var results = target.Get(1);

            //Evalute
            Assert.IsInstanceOf<SingleResult<Measurement>>(results);
            Assert.IsTrue(results.Queryable.Any());
        }

        [Test]
        public void GetSuccess_WithNoRecord()
        {
            //Setup
            var measurementServiceMock = new Mock<IMeasurementService>();
            var measurementServiceList = new List<Measurement>
            {
                new Measurement
                {
                    MeasurementId = 2
                }
            };
            IQueryable<Measurement> measurementQueryable = measurementServiceList.AsQueryable();
            measurementServiceMock.Setup(x => x.Read()).Returns(measurementQueryable);
            var dataModelMock = new Mock<IDataModel>();

            //Execute
            var target = new MeasurementController(measurementServiceMock.Object, dataModelMock.Object);
            var results = target.Get(1);

            //Evalute
            Assert.IsInstanceOf<SingleResult<Measurement>>(results);
            Assert.IsFalse(results.Queryable.Any());
        }

        [Test]
        public void PostSuccess()
        {
            //Setup
            var jobSiteSvcMock = new Mock<IJobSiteService>();
            jobSiteSvcMock.Setup(x => x.Create(It.IsAny<JobSite>()));
            var dataModelMock = new Mock<IDataModel>();

            //Execute
            var target = new JobSiteController(jobSiteSvcMock.Object, dataModelMock.Object);
            var results = target.Post(new JobSite());

            //Evalute
            dataModelMock.Verify(x => x.SaveChanges(), Times.Once);
            Assert.IsInstanceOf<CreatedODataResult<JobSite>>(results);
        }

        [Test]
        public void PostExceptionThrown()
        {
            //Setup
            var jobSiteSvcMock = new Mock<IJobSiteService>();
            jobSiteSvcMock.Setup(x => x.Delete(1)).Throws<Exception>();
            var dataModelMock = new Mock<IDataModel>();
            //Execute
            var target = new JobSiteController(jobSiteSvcMock.Object, dataModelMock.Object);
            var results = target.Delete(1);
            //Evaluate
            Assert.IsInstanceOf<ExceptionResult>(results);
        }
    }
}
