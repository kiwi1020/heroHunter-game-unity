using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//���� ���� ������
public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour
{
    public BattleManager battleManager;

    public Unit[] units; // 0 �÷��̾� // 1 �̻� ��
    public BattleHUD[] unitHUDs;

    public BattleState state;

    void Start()
    {  
        state = BattleState.START;
        SetupBattle();
    }

    public void StartBattle()
    {
    }

    void SetupBattle()
    {
        //Unit, HUD �迭 Ÿ������ ����
        foreach (Unit i in units) i.gameObject.SetActive(false);
        foreach (BattleHUD i in unitHUDs) i.gameObject.SetActive(false);

        #region Unit Setting

        //var tileData = (BattleTile)PlayManager.instance.curTile;

<<<<<<< Updated upstream
        for(int i = 0; i< tileData.enemies.Count+1; i++)
        {
            units[i].gameObject.SetActive(true);
            unitHUDs[i].gameObject.SetActive(true);
=======
        //enemyUnit.SetUnit(tileData.enemies[0]);
        enemyUnit.SetUnit();
>>>>>>> Stashed changes

            if(i == 0)
            {
                units[i].SetUnit();
                unitHUDs[i].SetHUD(units[i]);
            }
            else
            {
                units[i].SetUnit(tileData.enemies[i-1]);
                unitHUDs[i].SetHUD(units[i]);
            }
        }

        #endregion

        //start Player Turn
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        print("�÷��̾� ����");
        //������ �������� ������
        bool isDead = units[1].Takedamage(units[0].damage);

        unitHUDs[1].SetHP();
        //dialogueText.text = "���� ����!"

        yield return new WaitForSeconds(2f);

        //���� �׾������� Ȯ��
        if (isDead)
        {
            //����Ȯ�� �� ���� ���¸� ��ȭ��Ŵ
            state = BattleState.WON;
            Destroy(units[1]);
            EndBattle();
        }
    }

    IEnumerator EnemyTurn()
    {
        print("�� ����");
        //dialogueText.text = enemyUnit.unitName + "����!"

        yield return new WaitForSeconds(1f);

        bool isDead = units[0].Takedamage(units[1].damage);

        unitHUDs[0].SetHP();

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            Destroy(units[0]);
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if(state == BattleState.WON)
        {
            //dialogueText.text = "�¸�!";
            SceneManager.LoadScene("MoveScene");
        }
        else if (state == BattleState.LOST)
        {
            //dialogueText.text = "�й�..."
        }
    }

    void PlayerTurn()
    {
        //dialogueText.text = "ī�带 �����Ͻʽÿ�";
    }
    //�ϴ� ���ݹ�ư�� �ִٴ� �����Ͽ� ����
    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());
    }

    public void OnFinishButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }
}
