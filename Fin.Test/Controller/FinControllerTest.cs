using Fin.Api;
using Fin.Api.Model;
using Fin.Core.Models;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;
using static Fin.Core.Handlers.Command.CreateAdjustmentTransaction;
using static Fin.Core.Handlers.Command.CreatePaymentTransaction;
using static Fin.Core.Handlers.Query.GetAccountBalance;

namespace Fin.Test.Controller
{
    public class FinControllerTest : IClassFixture<TestFixture<Startup>>
    {
        //private Mock<IMediator> _mediator;
        private TestFixture<Startup> testFixture;

        public FinControllerTest(TestFixture<Startup> testFixture)
        {
            this.testFixture = testFixture;
            // this._mediator = new Mock<IMediator>();
        }

        [Fact]
        public async void GetAccountBalance_BadRequest()
        {
            var httpResponse = await this.testFixture.Client.GetAsync("/api");

            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);

            var responseAsString = await httpResponse.Content.ReadAsStringAsync();
            var actualResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<FinResponse>(responseAsString);

            Assert.NotNull(actualResponse);
            Assert.Equal("Invalid account number", actualResponse.ErrorMessage);
        }

        [Fact]
        public async void GetAccountBalance_NotFound()
        {
            var httpResponse = await this.testFixture.Client.GetAsync("/api?id=4789");

            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);

            var responseAsString = await httpResponse.Content.ReadAsStringAsync();
            var actualResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<FinResponse>(responseAsString);

            Assert.NotNull(actualResponse);
            Assert.Equal("Account not found!", actualResponse.ErrorMessage);
        }

        [Fact]
        public async void GetAccountBalance_Success()
        {
            var httpResponse = await this.testFixture.Client.GetAsync("/api?id=4755");

            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);

            var responseAsString = await httpResponse.Content.ReadAsStringAsync();
            var actualResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<FinResponse<AccountBalanceResponse>>(responseAsString, this.testFixture.jsonSettings);
            Assert.True(actualResponse.HasData);
            var data = actualResponse.GetData() as AccountBalanceResponse;
            Assert.NotNull(data);
                        
            Assert.Equal(1001.88m, data.Balance);
        }

        [Fact]
        public async void PostTransaction_AmountValidation()
        {

            CreateTransaction transaction = new CreateTransaction()
            {
                AccountId = 4343,
                Amount = 0,
                Origin = "VISA",
                MessageType = "PAYMENT"
            };
            var json = JsonConvert.SerializeObject(transaction);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await this.testFixture.Client.PostAsync("/api", content);

            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }

        [Fact]
        public async void PostTransaction_OriginValidation()
        {

            CreateTransaction transaction = new CreateTransaction()
            {
                AccountId = 4343,
                Amount = 10,
                Origin = "adad",
                MessageType = "PAYMENT"
            };
            var json = JsonConvert.SerializeObject(transaction);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await this.testFixture.Client.PostAsync("/api", content);

            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }

        [Fact]
        public async void PostTransaction_MessageTypeValidation()
        {

            CreateTransaction transaction = new CreateTransaction()
            {
                AccountId = 4343,
                Amount = 10,
                Origin = "VISA",
                MessageType = "aldaldk"
            };
            var json = JsonConvert.SerializeObject(transaction);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await this.testFixture.Client.PostAsync("/api", content);

            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }

        [Fact]
        public async void PostTransaction_PaymentAccountIdValidation()
        {

            CreateTransaction transaction = new CreateTransaction()
            {
                AccountId = 4343,
                Amount = 10,
                Origin = "VISA",
                MessageType = "PAYMENT"
            };
            var json = JsonConvert.SerializeObject(transaction);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await this.testFixture.Client.PostAsync("/api", content);

            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }

        [Fact]
        public async void PostTransaction_AdjustmentAccountIdValidation()
        {

            CreateTransaction transaction = new CreateTransaction()
            {
                AccountId = 4343,
                Amount = 10,
                Origin = "VISA",
                MessageType = "ADJUSTMENT"
            };
            var json = JsonConvert.SerializeObject(transaction);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await this.testFixture.Client.PostAsync("/api", content);

            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }

        [Fact]
        public async void PostTransaction_Balance_Insufficient()
        {

            CreateTransaction transaction = new CreateTransaction()
            {
                AccountId = 4755,
                Amount = 1005.55m,
                Origin = "VISA",
                MessageType = "PAYMENT"
            };
            var json = JsonConvert.SerializeObject(transaction);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await this.testFixture.Client.PostAsync("/api", content);
            var responseAsString = await httpResponse.Content.ReadAsStringAsync();
            var actualResponse = JsonConvert.DeserializeObject<FinResponse<CreatePaymentTransactionResponse>>(responseAsString, this.testFixture.jsonSettings);
            Assert.True(actualResponse.HasData);
            Assert.Equal("Balance is insufficient for this transaction", actualResponse.ErrorMessage);

            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }

        [Fact]
        public async void PostTransaction_Successfull_Payment()
        {

            CreateTransaction transaction = new CreateTransaction()
            {
                AccountId = 4755,
                Amount = 10,
                Origin = "VISA",
                MessageType = "PAYMENT"
            };
            var json = JsonConvert.SerializeObject(transaction);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await this.testFixture.Client.PostAsync("/api", content);
            var responseAsString = await httpResponse.Content.ReadAsStringAsync();
            var actualResponse = JsonConvert.DeserializeObject<FinResponse<CreatePaymentTransactionResponse>>(responseAsString, this.testFixture.jsonSettings);
            var data = actualResponse.GetData() as CreatePaymentTransactionResponse;
            Assert.NotNull(data);
            Assert.NotEqual(Guid.Empty, data.TransacitonId);
            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        }

        [Fact]
        public async void PostTransaction_Invalid_ParentTransactionId()
        {

            CreateTransaction transaction = new CreateTransaction()
            {
                AccountId = 4755,
                Amount = 10,
                Origin = "VISA",
                MessageType = "ADJUSTMENT"
            };
            var json = JsonConvert.SerializeObject(transaction);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await this.testFixture.Client.PostAsync("/api", content);
            var responseAsString = await httpResponse.Content.ReadAsStringAsync();
            var actualResponse = JsonConvert.DeserializeObject<FinResponse<CreateAdjustmentTransactionResponse>>(responseAsString, this.testFixture.jsonSettings);
            Assert.Equal("Transaction number to be adjusted is required", actualResponse.ErrorMessage);

            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }

        [Fact]
        public async void PostTransaction_Wrong_ParentTransactionId()
        {

            CreateTransaction transaction = new CreateTransaction()
            {
                AccountId = 4755,
                Amount = 10,
                Origin = "VISA",
                MessageType = "ADJUSTMENT",
                TransactionId = Guid.NewGuid().ToString()
            };
            var json = JsonConvert.SerializeObject(transaction);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await this.testFixture.Client.PostAsync("/api", content);
            var responseAsString = await httpResponse.Content.ReadAsStringAsync();
            var actualResponse = JsonConvert.DeserializeObject<FinResponse<CreateAdjustmentTransactionResponse>>(responseAsString, this.testFixture.jsonSettings);
            Assert.Equal("Transaction number to be adjusted is required", actualResponse.ErrorMessage);

            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }

        [Fact]
        public async void PostTransaction_Adjustment_Wrong_Amount()
        {

            CreateTransaction transaction = new CreateTransaction()
            {
                AccountId = 4755,
                Amount = 10,
                Origin = "VISA",
                MessageType = "PAYMENT"
            };
            var json = JsonConvert.SerializeObject(transaction);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await this.testFixture.Client.PostAsync("/api", content);
            var responseAsString = await httpResponse.Content.ReadAsStringAsync();
            var actualResponse = JsonConvert.DeserializeObject<FinResponse<CreatePaymentTransactionResponse>>(responseAsString, this.testFixture.jsonSettings);
            var data = actualResponse.GetData() as CreatePaymentTransactionResponse;

            CreateTransaction adjustmentTransaction = new CreateTransaction()
            {
                AccountId = 4755,
                Amount = 5,
                Origin = "VISA",
                MessageType = "ADJUSTMENT",
                TransactionId = data.TransacitonId.ToString()
            };
            var json2 = JsonConvert.SerializeObject(adjustmentTransaction);
            var content2 = new StringContent(json2, Encoding.UTF8, "application/json");

            var httpResponse2 = await this.testFixture.Client.PostAsync("/api", content2);
            var responseAsString2 = await httpResponse2.Content.ReadAsStringAsync();
            var actualResponse2 = JsonConvert.DeserializeObject<FinResponse<CreateAdjustmentTransactionResponse>>(responseAsString2, this.testFixture.jsonSettings);
            
            Assert.Equal("Partial Adjustment is not supported", actualResponse2.ErrorMessage);
            Assert.Equal(HttpStatusCode.BadRequest, httpResponse2.StatusCode);
        }

        [Fact]
        public async void PostTransaction_Adjustment_Origin_Validation()
        {

            CreateTransaction transaction = new CreateTransaction()
            {
                AccountId = 4755,
                Amount = 10,
                Origin = "VISA",
                MessageType = "PAYMENT"
            };
            var json = JsonConvert.SerializeObject(transaction);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await this.testFixture.Client.PostAsync("/api", content);
            var responseAsString = await httpResponse.Content.ReadAsStringAsync();
            var actualResponse = JsonConvert.DeserializeObject<FinResponse<CreatePaymentTransactionResponse>>(responseAsString, this.testFixture.jsonSettings);
            var data = actualResponse.GetData() as CreatePaymentTransactionResponse;

            CreateTransaction adjustmentTransaction = new CreateTransaction()
            {
                AccountId = 4755,
                Amount = 10,
                Origin = "MASTERCARD",
                MessageType = "ADJUSTMENT",
                TransactionId = data.TransacitonId.ToString()
            };
            var json2 = JsonConvert.SerializeObject(adjustmentTransaction);
            var content2 = new StringContent(json2, Encoding.UTF8, "application/json");

            var httpResponse2 = await this.testFixture.Client.PostAsync("/api", content2);
            var responseAsString2 = await httpResponse2.Content.ReadAsStringAsync();
            var actualResponse2 = JsonConvert.DeserializeObject<FinResponse<CreateAdjustmentTransactionResponse>>(responseAsString2, this.testFixture.jsonSettings);
            Assert.Equal("Transaction origin must be same with original transaction", actualResponse2.ErrorMessage);
            Assert.Equal(HttpStatusCode.BadRequest, httpResponse2.StatusCode);
        }

        [Fact]
        public async void PostTransaction_Adjustment_Success()
        {

            CreateTransaction transaction = new CreateTransaction()
            {
                AccountId = 4755,
                Amount = 10,
                Origin = "VISA",
                MessageType = "PAYMENT"
            };
            var json = JsonConvert.SerializeObject(transaction);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await this.testFixture.Client.PostAsync("/api", content);
            var responseAsString = await httpResponse.Content.ReadAsStringAsync();
            var actualResponse = JsonConvert.DeserializeObject<FinResponse<CreatePaymentTransactionResponse>>(responseAsString, this.testFixture.jsonSettings);
            var data = actualResponse.GetData() as CreatePaymentTransactionResponse;

            CreateTransaction adjustmentTransaction = new CreateTransaction()
            {
                AccountId = 4755,
                Amount = 10,
                Origin = "VISA",
                MessageType = "ADJUSTMENT",
                TransactionId = data.TransacitonId.ToString()
            };
            var json2 = JsonConvert.SerializeObject(adjustmentTransaction);
            var content2 = new StringContent(json2, Encoding.UTF8, "application/json");

            var httpResponse2 = await this.testFixture.Client.PostAsync("/api", content2);
            var responseAsString2 = await httpResponse2.Content.ReadAsStringAsync();
            var actualResponse2 = JsonConvert.DeserializeObject<FinResponse<CreateAdjustmentTransactionResponse>>(responseAsString2, this.testFixture.jsonSettings);

            Assert.Equal(HttpStatusCode.OK, httpResponse2.StatusCode);
        }   
    }
}
