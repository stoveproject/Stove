using System;

namespace Stove.Application.Dto
{
    /// <summary>
    /// Simply implements <see cref="IPagedAndSortedResultRequest"/>.
    /// </summary>
    [Serializable]
    public class PagedAndSortedResultRequestDto : PagedResultRequestDto, IPagedAndSortedResultRequest
    {
        public virtual string Sorting { get; set; }
    }
}