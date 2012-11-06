namespace Core.Model
{
	public class User : Entity
	{
		public virtual string Name { get; set; }
		public virtual string EmailAddress { get; set; }
	}
}
