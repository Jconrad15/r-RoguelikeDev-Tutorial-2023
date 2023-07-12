using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    protected bool inInventory = false;

    public void MoveToInventory()
    {
        inInventory = true;
    }

    public void RemoveFromInventory()
    {
        inInventory = false;
    }

    public virtual bool TryActivate(Entity usingEntity)
    {
        return false;
    }

    protected void OnMouseOver()
    {
        if (inInventory == false) { return; }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("leftclick");
            EntityManager em = FindAnyObjectByType<EntityManager>();
            if (TryActivate(em.Player))
            {
                Entity entity = GetComponent<Entity>();
                Inventory inv = em.Player.GetComponent<Inventory>();
                inv.Remove(entity);
                em.DestroyEntity(entity);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            EntityManager em = FindAnyObjectByType<EntityManager>();
            Debug.Log("rightclick");
            Inventory inv = em.Player.GetComponent<Inventory>();
            inv.Drop(GetComponent<Entity>());
        }
    }

}
