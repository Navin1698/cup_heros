using System.Collections;
using UnityEngine;
using OrbRaiders.Combat;

namespace OrbRaiders.Bosses
{
    public class EmberGolemBoss : BossBase
    {
        protected override IEnumerator BossLoopRoutine()
        {
            yield return new WaitForSeconds(1.5f); // Brief intro pause

            while (IsAlive)
            {
                if (playerTransform == null && Player.PlayerController.Instance != null)
                {
                    playerTransform = Player.PlayerController.Instance.transform;
                }

                if (playerTransform != null)
                {
                    // Move towards player
                    Vector3 moveDir = (playerTransform.position - transform.position).normalized;
                    transform.position += moveDir * (definition != null ? definition.moveSpeed : 2.0f) * Time.deltaTime;
                    transform.rotation = Quaternion.LookRotation(moveDir);

                    // Pick random ability
                    float rand = Random.value;
                    if (CurrentPhase == 1)
                    {
                        if (rand < 0.4f) yield return ExecuteGroundSlam();
                        else if (rand < 0.7f) yield return ExecuteFireRing();
                        else yield return ExecuteRockThrow();
                    }
                    else
                    {
                        // Phase 2 abilities
                        if (rand < 0.35f) yield return ExecuteMeteorBurst();
                        else if (rand < 0.7f) yield return ExecuteCharge();
                        else yield return ExecuteFireRing();
                    }
                }

                yield return new WaitForSeconds(2.0f);
            }
        }

        // Ability 1: Ground Slam (Telegraphed Circle)
        private IEnumerator ExecuteGroundSlam()
        {
            isAttacking = true;
            Vector3 targetPos = playerTransform != null ? playerTransform.position : transform.position;

            bool finishedTelegraph = false;
            TelegraphSystem.Instance?.ShowCircleTelegraph(targetPos, 4.0f, 1.2f, () => finishedTelegraph = true);

            yield return new WaitUntil(() => finishedTelegraph);

            // Deal AOE damage at targetPos
            Collider[] hits = Physics.OverlapSphere(targetPos, 4.0f);
            foreach (var hit in hits)
            {
                var ph = hit.GetComponent<Player.PlayerHealth>();
                if (ph != null)
                {
                    ph.TakeDamage(new DamageResult
                    {
                        amount = definition != null ? definition.baseDamage * 1.5f : 35f,
                        type = DamageType.Physical,
                        source = gameObject,
                        target = ph.gameObject,
                        knockbackDirection = (ph.transform.position - targetPos).normalized,
                        knockbackForce = 5f
                    });
                }
            }
            isAttacking = false;
        }

        // Ability 2: Fire Ring
        private IEnumerator ExecuteFireRing()
        {
            isAttacking = true;
            int projectileCount = 12;
            float step = 360f / projectileCount;

            for (int i = 0; i < projectileCount; i++)
            {
                float angle = i * step;
                Vector3 dir = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;

                GameObject fireOrb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                fireOrb.transform.position = transform.position + Vector3.up * 1.0f;
                fireOrb.transform.localScale = Vector3.one * 0.6f;

                var proj = fireOrb.AddComponent<Projectile>();
                proj.Initialize(dir, 6f, definition != null ? definition.baseDamage : 20f, false, 0, 0, false, true, gameObject);
            }

            yield return new WaitForSeconds(1.0f);
            isAttacking = false;
        }

        // Ability 3: Rock Throw
        private IEnumerator ExecuteRockThrow()
        {
            isAttacking = true;
            if (playerTransform != null)
            {
                Vector3 dir = (playerTransform.position - transform.position).normalized;
                GameObject rock = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                rock.transform.position = transform.position + Vector3.up * 1.2f;
                rock.transform.localScale = Vector3.one * 1.2f;

                var proj = rock.AddComponent<Projectile>();
                proj.Initialize(dir, 9f, definition != null ? definition.baseDamage * 1.2f : 25f, false, 0, 0, false, false, gameObject);
            }
            yield return new WaitForSeconds(0.5f);
            isAttacking = false;
        }

        // Ability 4: Charge (Line Telegraph)
        private IEnumerator ExecuteCharge()
        {
            isAttacking = true;
            if (playerTransform != null)
            {
                Vector3 chargeDir = (playerTransform.position - transform.position).normalized;
                bool finishedLine = false;

                TelegraphSystem.Instance?.ShowLineTelegraph(transform.position, chargeDir, 12f, 2.5f, 1.0f, () => finishedLine = true);
                yield return new WaitUntil(() => finishedLine);

                // Rush forward
                float elapsed = 0f;
                while (elapsed < 0.6f)
                {
                    elapsed += Time.deltaTime;
                    transform.position += chargeDir * 20f * Time.deltaTime;
                    yield return null;
                }
            }
            isAttacking = false;
        }

        // Phase 2 Special: Meteor Burst
        private IEnumerator ExecuteMeteorBurst()
        {
            isAttacking = true;
            Debug.Log("[EmberGolem] METEOR BURST!");

            for (int i = 0; i < 5; i++)
            {
                Vector3 randomImpact = transform.position + new Vector3(Random.Range(-8f, 8f), 0, Random.Range(-8f, 8f));
                bool finished = false;

                TelegraphSystem.Instance?.ShowCircleTelegraph(randomImpact, 3.0f, 0.8f, () => finished = true);
                yield return new WaitUntil(() => finished);

                Collider[] hits = Physics.OverlapSphere(randomImpact, 3.0f);
                foreach (var hit in hits)
                {
                    var ph = hit.GetComponent<Player.PlayerHealth>();
                    if (ph != null)
                    {
                        ph.TakeDamage(new DamageResult
                        {
                            amount = definition != null ? definition.baseDamage * 2.0f : 45f,
                            type = DamageType.Fire,
                            source = gameObject,
                            target = ph.gameObject
                        });
                    }
                }
            }
            isAttacking = false;
        }
    }
}
