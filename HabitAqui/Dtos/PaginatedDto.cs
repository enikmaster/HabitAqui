namespace HabitAqui.Dtos
{
    public class PaginatedDto<T>
    {
        public T Value { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
        public int? Id { get; set; }
    }
}
