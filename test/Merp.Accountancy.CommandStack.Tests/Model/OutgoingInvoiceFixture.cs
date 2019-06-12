using Merp.Accountancy.CommandStack.Model;
using Merp.Accountancy.CommandStack.Services;
using Moq;
using SharpTestsEx;
using System;
using System.Collections.Generic;
using Xunit;

namespace Merp.Accountancy.CommandStack.Tests.Model
{
    public class OutgoingInvoiceFixture
    {
        public class Factory
        {
            [Fact]
            public void Issue_should_throw_ArgumentNullException_if_outgoingInvoiceNumberGenerator_is_null()
            {
                var lineItems = new Invoice.InvoiceLineItem[0];
                var pricesByVat = new Invoice.InvoicePriceByVat[0];
                var nonTaxableItems = new Invoice.NonTaxableItem[0];

                Executing.This(() => OutgoingInvoice.Factory.Issue(
                    null,
                    DateTime.Today,
                    "EUR",
                    1000,
                    100,
                    1100,
                    1100,
                    "description",
                    string.Empty,
                    string.Empty,
                    Guid.NewGuid(),
                    "Customer",
                    "customer address",
                    "customer city",
                    "12345",
                    "IT",
                    "12345678900",
                    "12345678900",
                    "Supplier",
                    "supplier address",
                    "supplier city",
                    "12345",
                    "IT",
                    "12345678900",
                    "12345678900",
                    lineItems,
                    false,
                    pricesByVat,
                    nonTaxableItems,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    Guid.NewGuid()))
                    .Should()
                    .Throw<ArgumentNullException>()
                    .And
                    .ValueOf
                    .ParamName
                    .Should()
                    .Be
                    .EqualTo("generator");
            }

            [Fact]
            public void Issue_should_throw_ArgumentNullException_if_lineItems_are_null()
            {
                var generator = new Mock<IOutgoingInvoiceNumberGenerator>().Object;
                IEnumerable<Invoice.InvoiceLineItem> lineItems = null;
                var pricesByVat = new Invoice.InvoicePriceByVat[0];
                var nonTaxableItems = new Invoice.NonTaxableItem[0];

                Executing.This(() => OutgoingInvoice.Factory.Issue(
                    generator,
                    DateTime.Today,
                    "EUR",
                    1000,
                    100,
                    1100,
                    1100,
                    "description",
                    string.Empty,
                    string.Empty,
                    Guid.NewGuid(),
                    "Customer",
                    "customer address",
                    "customer city",
                    "12345",
                    "IT",
                    "12345678900",
                    "12345678900",
                    "Supplier",
                    "supplier address",
                    "supplier city",
                    "12345",
                    "IT",
                    "12345678900",
                    "12345678900",
                    lineItems,
                    false,
                    pricesByVat,
                    nonTaxableItems,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    Guid.NewGuid()))
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
            public void Issue_should_throw_ArgumentNullException_if_pricesByVat_are_null()
            {
                var generator = new Mock<IOutgoingInvoiceNumberGenerator>().Object;
                var lineItems = new Invoice.InvoiceLineItem[0];
                IEnumerable<Invoice.InvoicePriceByVat> pricesByVat = null;
                var nonTaxableItems = new Invoice.NonTaxableItem[0];

                Executing.This(() => OutgoingInvoice.Factory.Issue(
                    generator,
                    DateTime.Today,
                    "EUR",
                    1000,
                    100,
                    1100,
                    1100,
                    "description",
                    string.Empty,
                    string.Empty,
                    Guid.NewGuid(),
                    "Customer",
                    "customer address",
                    "customer city",
                    "12345",
                    "IT",
                    "12345678900",
                    "12345678900",
                    "Supplier",
                    "supplier address",
                    "supplier city",
                    "12345",
                    "IT",
                    "12345678900",
                    "12345678900",
                    lineItems,
                    false,
                    pricesByVat,
                    nonTaxableItems,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    Guid.NewGuid()))
                    .Should()
                    .Throw<ArgumentNullException>()
                    .And
                    .ValueOf
                    .ParamName
                    .Should()
                    .Be
                    .EqualTo(nameof(pricesByVat));
            }

            [Fact]
            public void Register_should_throw_ArgumentException_if_invoiceNumber_is_null_or_whitespace()
            {
                var lineItems = new Invoice.InvoiceLineItem[0];
                var pricesByVat = new Invoice.InvoicePriceByVat[0];
                var nonTaxableItems = new Invoice.NonTaxableItem[0];

                Executing.This(() => OutgoingInvoice.Factory.Register(
                    null,
                    DateTime.Today,
                    DateTime.Today.AddMonths(1),
                    "EUR",
                    1000,
                    100,
                    1100,
                    1100,
                    "description",
                    string.Empty,
                    string.Empty,
                    Guid.NewGuid(),
                    "Customer",
                    "customer address",
                    "customer city",
                    "12345",
                    "IT",
                    "12345678900",
                    "12345678900",
                    "Supplier",
                    "supplier address",
                    "supplier city",
                    "12345",
                    "IT",
                    "12345678900",
                    "12345678900",
                    lineItems,
                    false,
                    pricesByVat,
                    nonTaxableItems,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    Guid.NewGuid()))
                    .Should()
                    .Throw<ArgumentException>()
                    .And
                    .ValueOf
                    .ParamName
                    .Should()
                    .Be
                    .EqualTo("invoiceNumber");
            }

            [Fact]
            public void Register_should_throw_ArgumentNullException_if_lineItems_are_null()
            {
                IEnumerable<Invoice.InvoiceLineItem> lineItems = null;
                var pricesByVat = new Invoice.InvoicePriceByVat[0];
                var nonTaxableItems = new Invoice.NonTaxableItem[0];

                Executing.This(() => OutgoingInvoice.Factory.Register(
                    "1",
                    DateTime.Today,
                    DateTime.Today.AddMonths(1),
                    "EUR",
                    1000,
                    100,
                    1100,
                    1100,
                    "description",
                    string.Empty,
                    string.Empty,
                    Guid.NewGuid(),
                    "Customer",
                    "customer address",
                    "customer city",
                    "12345",
                    "IT",
                    "12345678900",
                    "12345678900",
                    "Supplier",
                    "supplier address",
                    "supplier city",
                    "12345",
                    "IT",
                    "12345678900",
                    "12345678900",
                    lineItems,
                    false,
                    pricesByVat,
                    nonTaxableItems,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    Guid.NewGuid()))
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
                var lineItems = new Invoice.InvoiceLineItem[0];
                IEnumerable<Invoice.InvoicePriceByVat> pricesByVat = null;
                var nonTaxableItems = new Invoice.NonTaxableItem[0];

                Executing.This(() => OutgoingInvoice.Factory.Register(
                    "1",
                    DateTime.Today,
                    DateTime.Today.AddMonths(1),
                    "EUR",
                    1000,
                    100,
                    1100,
                    1100,
                    "description",
                    string.Empty,
                    string.Empty,
                    Guid.NewGuid(),
                    "Customer",
                    "customer address",
                    "customer city",
                    "12345",
                    "IT",
                    "12345678900",
                    "12345678900",
                    "Supplier",
                    "supplier address",
                    "supplier city",
                    "12345",
                    "IT",
                    "12345678900",
                    "12345678900",
                    lineItems,
                    false,
                    pricesByVat,
                    nonTaxableItems,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    Guid.NewGuid()))
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
