using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public List<Quest> availableQuests = new List<Quest>();
    public List<Quest> activeQuests = new List<Quest>();
    public List<Quest> completedQuests = new List<Quest>();
    
    public void Start()
    {
        // Inicializar algumas quests básicas
        CreateDefaultQuests();
    }
    
    private void CreateDefaultQuests()
    {
        // Quest para matar inimigos
        Quest killQuest = new Quest(
            "kill_enemies_01", // ID único
            "Limpar o Caminho",
            "Derrote 10 inimigos na zona inicial.",
            QuestType.KillEnemies,
            10
        );
        
        QuestReward killReward = new QuestReward();
        killReward.gold = 50;
        killReward.experience = 100;
        killQuest.rewards.Add(killReward);
        
        // Quest para coletar itens
        Quest collectQuest = new Quest(
            "collect_potions_01", // ID único
            "Colecionador",
            "Colete 5 poções de cura.",
            QuestType.CollectItems,
            5
        );
        
        QuestReward collectReward = new QuestReward();
        collectReward.gold = 30;
        collectReward.experience = 50;
        Item rewardItem = new Item("Anel de Proteção", "Aumenta a defesa do usuário", ItemType.Gloves, ItemRarity.Uncommon, 1);
        rewardItem.vitalityModifier = 3;
        collectReward.item = rewardItem;
        collectQuest.rewards.Add(collectReward);
        
        // Quest para derrotar um boss (nova)
        Quest bossQuest = new Quest(
            "defeat_boss_01", // ID único
            "A Ameaça Final",
            "Derrote o Chefe Goblin que está aterrorizando o vilarejo.",
            QuestType.DefeatBoss,
            1
        );
        
        QuestReward bossReward = new QuestReward();
        bossReward.gold = 200;
        bossReward.experience = 500;
        Item bossItem = new Item("Espada do Exterminador", "Uma arma poderosa forjada em fogo antigo", ItemType.Weapon, ItemRarity.Epic, 10);
        bossItem.physicalDamage = 25;
        bossItem.strengthModifier = 5;
        bossReward.item = bossItem;
        bossQuest.rewards.Add(bossReward);
        
        // Adicionar quests à lista
        availableQuests.Add(killQuest);
        availableQuests.Add(collectQuest);
        availableQuests.Add(bossQuest);
    }
    
    public void AcceptQuest(Quest quest)
    {
        if (availableQuests.Contains(quest))
        {
            availableQuests.Remove(quest);
            activeQuests.Add(quest);
            Debug.Log("Quest aceita: " + quest.title);
        }
    }
    
    public void UpdateQuestProgress(QuestType type, int amount = 1)
    {
        foreach (Quest quest in activeQuests)
        {
            if (quest.type == type && !quest.isCompleted)
            {
                quest.UpdateProgress(amount);
                
                // Verificar conclusão
                if (quest.isCompleted)
                {
                    activeQuests.Remove(quest);
                    completedQuests.Add(quest);
                    break;
                }
            }
        }
    }
    
    public void AbandonQuest(Quest quest)
    {
        if (activeQuests.Contains(quest))
        {
            activeQuests.Remove(quest);
            availableQuests.Add(quest);
            
            // Resetar progresso
            quest.currentAmount = 0;
            
            Debug.Log("Quest abandonada: " + quest.title);
        }
    }
    
    // Método para buscar quest pelo ID
    public Quest GetQuestByID(string questID)
    {
        // Verificar todas as listas de quests
        foreach (Quest quest in availableQuests)
        {
            if (quest.id == questID)
                return quest;
        }
        
        foreach (Quest quest in activeQuests)
        {
            if (quest.id == questID)
                return quest;
        }
        
        foreach (Quest quest in completedQuests)
        {
            if (quest.id == questID)
                return quest;
        }
        
        return null; // Não encontrado
    }
    
    // Método para depuração
    public void DebugPrintAllQuests()
    {
        Debug.Log("===== TODAS AS QUESTS =====");
        
        Debug.Log("Quests Disponíveis:");
        foreach (Quest quest in availableQuests)
        {
            Debug.Log($"- {quest.id}: {quest.title} ({quest.type})");
        }
        
        Debug.Log("Quests Ativas:");
        foreach (Quest quest in activeQuests)
        {
            Debug.Log($"- {quest.id}: {quest.title} - Progresso: {quest.currentAmount}/{quest.requiredAmount}");
        }
        
        Debug.Log("Quests Completadas:");
        foreach (Quest quest in completedQuests)
        {
            Debug.Log($"- {quest.id}: {quest.title}");
        }
    }
}