public interface IActorTemplate
{
	int SendDamage();
	void TakeDamage(int incomingDamage);
	void Die();
	void ActorStats(SOActorModel actorModel);
}