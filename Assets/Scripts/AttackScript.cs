using System.Collections;
using UnityEngine;
/// <summary>
/// @alex-memo 2023
/// This class is responsible for 
/// </summary>
public class AttackScript : MonoBehaviour
{
	private Animator anim;
	private MovementScript movement;
	private int comboCount = 0;
	private bool isAttacking = false;
	private void Awake()
	{
		anim = GetComponent<Animator>();
		movement = GetComponent<MovementScript>();
	}
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			movement.resetIdleTimer();
			if (isAttacking){return;}
			StartCoroutine(attackRoutine());
		}
	}
	private void attack()
	{
		movement.CanMove = false;
		setCombo();
		anim.SetTrigger("Attack"+comboCount);
		++comboCount;
	}
	private IEnumerator attackRoutine()
	{
		isAttacking = true;
		attack();		
		yield return new WaitForSeconds(.5f);
		float _timer=0f;
		while(_timer<.5f)
		{
			_timer += Time.deltaTime;
			if (Input.GetMouseButtonDown(0))
			{
				attack();
				_timer = 0f;
				yield return new WaitForSeconds(.5f);
			}
			yield return null;
		}
		isAttacking = false;
		resetCombo();
	}
	private void resetCombo()
	{
		comboCount = 0;
	}
	private void setCombo()
	{
		if(comboCount>4){comboCount=0;}
		//anim.SetInteger("Combo", comboCount);
	}
}