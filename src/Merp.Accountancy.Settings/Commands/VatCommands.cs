using Merp.Accountancy.Settings.Model;
using System;
using System.Threading.Tasks;

namespace Merp.Accountancy.Settings.Commands
{
    public class VatCommands : IDisposable
    {
        private readonly AccountancySettingsDbContext _context;

        public VatCommands(AccountancySettingsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateVat(Guid subscriptionId, string country, decimal rate, string description)
        {
            if (subscriptionId == Guid.Empty)
            {
                throw new ArgumentException("value cannot be empty", nameof(subscriptionId));
            }

            if (string.IsNullOrWhiteSpace(country))
            {
                throw new ArgumentException("value cannot be empty", nameof(country));
            }

            if (rate < 0)
            {
                throw new ArgumentException("value cannot be less than zero", nameof(rate));
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("value cannot be empty", nameof(description));
            }

            var vat = new Vat
            {
                Id = Guid.NewGuid(),
                SubscriptionId = subscriptionId,
                Country = country,
                Description = description,
                Rate = rate,
                Unlisted = false
            };

            _context.Add(vat);
            await _context.SaveChangesAsync();
        }

        public async Task EditVat(Guid vatId, decimal rate, string description)
        {
            if (vatId == Guid.Empty)
            {
                throw new ArgumentException("value cannot be empty", nameof(vatId));
            }

            if (rate < 0)
            {
                throw new ArgumentException("value cannot be less than zero", nameof(rate));
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("value cannot be empty", nameof(description));
            }

            var vat = await _context.FindAsync<Vat>(vatId);
            if (vat.Unlisted)
            {
                throw new InvalidOperationException("Cannot edit unlisted vat");
            }

            if (vat.Rate != rate)
            {
                vat.Rate = rate;
            }

            if (vat.Description != description)
            {
                vat.Description = description;
            }

            await _context.SaveChangesAsync();
        }

        public async Task UnlistVat(Guid vatId)
        {
            if (vatId == Guid.Empty)
            {
                throw new ArgumentException("value cannot be empty", nameof(vatId));
            }

            var vat = await _context.FindAsync<Vat>(vatId);
            vat.Unlisted = true;

            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }
    }
}
