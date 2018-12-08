using Merp.Accountancy.CommandStack.Model;
using SharpTestsEx;
using System;
using System.Collections.Generic;
using Xunit;

namespace Merp.Accountancy.CommandStack.Tests.Model
{
    public class IncomingInvoiceFixture
    {
        public class Factory
        {
            [Fact]
            public void Register_should_throw_ArgumentException_if_invoiceNumber_is_null_or_empty()
            {
                string invoiceNumber = string.Empty;
                var userId = Guid.NewGuid();
                var invoiceDate = DateTime.Today;
                var dueDate = invoiceDate.AddMonths(1);
                string currency = "EUR";
                decimal amount = 101;
                decimal taxes = 42;
                decimal totalPrice = 143;
                string description = "fake";
                string paymentTerms = "none";
                string purchaseOrderNumber = "42";
                var customerId = Guid.NewGuid();
                string customerName = "Managed Designs S.r.l.";
                string customerAddress = "Via Torino 51";
                string customerCity = "Milan";
                string customerPostalCode = "20123";
                string customerCountry = "Italy";
                string customerVatIndex = "04358780965";
                string customerNationalIdentificationNumber = "04358780965";
                var supplierId = Guid.NewGuid();
                string supplierName = "Mastreeno ltd";
                string supplierAddress = "8, Leman street";
                string supplierCity = "London";
                string supplierPostalCode = "";
                string supplierCountry = "England - United Kingdom";
                string supplierVatIndex = "XYZ";
                string supplierNationalIdentificationNumber = "04358780965";
                var lineItems = new Invoice.InvoiceLineItem[0];
                var pricesByVat = new Invoice.InvoicePriceByVat[0];
                var nonTaxableItems = new Invoice.NonTaxableItem[0];

                Executing.This(() => IncomingInvoice.Factory.Register(
                    invoiceNumber,
                    invoiceDate,
                    dueDate,
                    currency,
                    amount,
                    taxes,
                    totalPrice,
                    description,
                    paymentTerms,
                    purchaseOrderNumber,
                    customerId,
                    customerName,
                    customerAddress,
                    customerCity,
                    customerPostalCode,
                    customerCountry,
                    customerVatIndex,
                    customerNationalIdentificationNumber,
                    supplierId,
                    supplierName,
                    supplierAddress,
                    supplierCity,
                    supplierPostalCode,
                    supplierCountry,
                    supplierVatIndex,
                    supplierNationalIdentificationNumber,
                    lineItems,
                    false,
                    pricesByVat,
                    nonTaxableItems,
                    userId))
                    .Should()
                    .Throw<ArgumentException>()
                    .And
                    .ValueOf
                    .ParamName
                    .Should()
                    .Be
                    .EqualTo(nameof(invoiceNumber));
            }

            [Fact]
            public void Register_should_throw_ArgumentNullException_if_lineItems_are_null()
            {
                string invoiceNumber = "1";
                var userId = Guid.NewGuid();
                var invoiceDate = DateTime.Today;
                var dueDate = invoiceDate.AddMonths(1);
                string currency = "EUR";
                decimal amount = 101;
                decimal taxes = 42;
                decimal totalPrice = 143;
                string description = "fake";
                string paymentTerms = "none";
                string purchaseOrderNumber = "42";
                var customerId = Guid.NewGuid();
                string customerName = "Managed Designs S.r.l.";
                string customerAddress = "Via Torino 51";
                string customerCity = "Milan";
                string customerPostalCode = "20123";
                string customerCountry = "Italy";
                string customerVatIndex = "04358780965";
                string customerNationalIdentificationNumber = "04358780965";
                var supplierId = Guid.NewGuid();
                string supplierName = "Mastreeno ltd";
                string supplierAddress = "8, Leman street";
                string supplierCity = "London";
                string supplierPostalCode = "";
                string supplierCountry = "England - United Kingdom";
                string supplierVatIndex = "XYZ";
                string supplierNationalIdentificationNumber = "04358780965";
                IEnumerable<Invoice.InvoiceLineItem> lineItems = null;
                var pricesByVat = new Invoice.InvoicePriceByVat[0];
                var nonTaxableItems = new Invoice.NonTaxableItem[0];

                Executing.This(() => IncomingInvoice.Factory.Register(
                    invoiceNumber,
                    invoiceDate,
                    dueDate,
                    currency,
                    amount,
                    taxes,
                    totalPrice,
                    description,
                    paymentTerms,
                    purchaseOrderNumber,
                    customerId,
                    customerName,
                    customerAddress,
                    customerCity,
                    customerPostalCode,
                    customerCountry,
                    customerVatIndex,
                    customerNationalIdentificationNumber,
                    supplierId,
                    supplierName,
                    supplierAddress,
                    supplierCity,
                    supplierPostalCode,
                    supplierCountry,
                    supplierVatIndex,
                    supplierNationalIdentificationNumber,
                    lineItems,
                    false,
                    pricesByVat,
                    nonTaxableItems,
                    userId))
                    .Should()
                    .Throw<ArgumentNullException>()
                    .And
                    .ValueOf
                    .ParamName
                    .Should()
                    .Be
                    .EqualTo(nameof(lineItems));
            }

            [Fact]
            public void Register_should_throw_ArgumentNullException_if_pricesByVat_are_null()
            {
                string invoiceNumber = "1";
                var userId = Guid.NewGuid();
                var invoiceDate = DateTime.Today;
                var dueDate = invoiceDate.AddMonths(1);
                string currency = "EUR";
                decimal amount = 101;
                decimal taxes = 42;
                decimal totalPrice = 143;
                string description = "fake";
                string paymentTerms = "none";
                string purchaseOrderNumber = "42";
                var customerId = Guid.NewGuid();
                string customerName = "Managed Designs S.r.l.";
                string customerAddress = "Via Torino 51";
                string customerCity = "Milan";
                string customerPostalCode = "20123";
                string customerCountry = "Italy";
                string customerVatIndex = "04358780965";
                string customerNationalIdentificationNumber = "04358780965";
                var supplierId = Guid.NewGuid();
                string supplierName = "Mastreeno ltd";
                string supplierAddress = "8, Leman street";
                string supplierCity = "London";
                string supplierPostalCode = "";
                string supplierCountry = "England - United Kingdom";
                string supplierVatIndex = "XYZ";
                string supplierNationalIdentificationNumber = "04358780965";
                var lineItems = new Invoice.InvoiceLineItem[0];
                IEnumerable<Invoice.InvoicePriceByVat> pricesByVat = null;
                var nonTaxableItems = new Invoice.NonTaxableItem[0];

                Executing.This(() => IncomingInvoice.Factory.Register(
                    invoiceNumber,
                    invoiceDate,
                    dueDate,
                    currency,
                    amount,
                    taxes,
                    totalPrice,
                    description,
                    paymentTerms,
                    purchaseOrderNumber,
                    customerId,
                    customerName,
                    customerAddress,
                    customerCity,
                    customerPostalCode,
                    customerCountry,
                    customerVatIndex,
                    customerNationalIdentificationNumber,
                    supplierId,
                    supplierName,
                    supplierAddress,
                    supplierCity,
                    supplierPostalCode,
                    supplierCountry,
                    supplierVatIndex,
                    supplierNationalIdentificationNumber,
                    lineItems,
                    false,
                    pricesByVat,
                    nonTaxableItems,
                    userId))
                    .Should()
                    .Throw<ArgumentNullException>()
                    .And
                    .ValueOf
                    .ParamName
                    .Should()
                    .Be
                    .EqualTo(nameof(pricesByVat));
            }
        }
    }
}
