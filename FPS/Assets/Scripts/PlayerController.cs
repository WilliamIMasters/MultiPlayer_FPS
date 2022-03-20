using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks, IDamagable
{
    [SerializeField] Image healthBarImage;
    [SerializeField] GameObject ui;

    [SerializeField] GameObject cameraHolder;

    [SerializeField] float mouseSpeed, sprintSpeed, walkSpeed, jumpForce, smoothTime;

    [SerializeField] Item[] items;

    [SerializeField] ScoreBoard scoreBoard;

    int itemIndex;
    int prevItemIndex = -1;

    float verticalLookRotation;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    Rigidbody rb;
    PhotonView PV;

    const float maxHealth = 100f;
    float currentHealth = maxHealth;

    PlayerManager playerManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();

        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
        //scoreBoard = GameObject.FindGameObjectWithTag("ScoreBoard")?.GetComponent<ScoreBoard>();
        //scoreBoard.hide();
    }

    private void Start()
    {
        if(PV.IsMine) {
            EquipItem(0);
        }
        else {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
            Destroy(ui);
        }
    }

    private void Update()
    {
        if (!PV.IsMine)
            return;


        look();
        move();
        jump();
        handleSwappingWeapons();
        scoreBoardControlls();


        if (Input.GetMouseButtonDown(0)) { // use heald item
            items[itemIndex].Use();
        }



        if (transform.position.y  < -10f) { // kill if fall of map
            Die();
        }
        
    
    }

    void scoreBoardControlls()
    {
        if(Input.GetKeyDown(KeyCode.Tab)) {
            //scoreBoard.show();
        } else if (Input.GetKeyUp(KeyCode.Tab)) {
            //scoreBoard.hide();
        }
    }
    void handleSwappingWeapons()
    {
        for (int i = 0; i < items.Length; i++) {
            if (Input.GetKeyDown((i + 1).ToString())) {
                EquipItem(i);
                break;
            }
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f) {
            if (itemIndex >= items.Length - 1) {
                EquipItem(0);
            } else {
                EquipItem(itemIndex + 1);
            }

        } else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f) {
            if (itemIndex <= 0) {
                EquipItem(items.Length - 1);
            } else {
                EquipItem(itemIndex - 1);
            }

        }
    }
    void jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded) {
            rb.AddForce(transform.up * jumpForce);
        }
    }
    void move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);

    }
    void look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSpeed);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSpeed;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    void EquipItem(int index)
    {

        if (index == prevItemIndex) return;

        itemIndex = index;
        items[itemIndex].itemGameObject.SetActive(true);

        if(prevItemIndex != -1) {
            items[prevItemIndex].itemGameObject.SetActive(false);
        }

        prevItemIndex = itemIndex;



        if(PV.IsMine) {
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(!PV.IsMine && targetPlayer ==  PV.Owner) {
            EquipItem((int)changedProps["itemIndex"]);
        }
    }



    public void SetGroundedState(bool grounded)
    {
        this.grounded = grounded;
    }

    private void FixedUpdate()
    {
        if (!PV.IsMine)
            return;

        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    public void TakeDamage(float damage)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if(!PV.IsMine)
            return;

        Debug.Log("Took damage: " + damage);
        currentHealth -= damage;

        healthBarImage.fillAmount = currentHealth / maxHealth;

        if(currentHealth <= 0f) {
            Die();
        }
        
    }

    void Die()
    {
        playerManager.Die();
    }
}
