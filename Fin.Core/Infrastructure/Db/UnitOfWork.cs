using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fin.Core.Infrastructure.Db
{
    public sealed class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly FinContext _context;
        private bool _disposed;

        public UnitOfWork(FinContext context) => this._context = context;

        /// <inheritdoc />
        public void Dispose() => this.Dispose(true);

        /// <inheritdoc />
        public async Task<int> Save()
        {
            int affectedRows = await this._context
                .SaveChangesAsync()
                .ConfigureAwait(false);
            return affectedRows;
        }

        private void Dispose(bool disposing)
        {
            if (!this._disposed && disposing)
            {
                this._context.Dispose();
            }

            this._disposed = true;
        }
    }
}
