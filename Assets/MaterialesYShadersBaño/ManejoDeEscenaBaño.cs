using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManejoDeEscenaBaño : MonoBehaviour
{
    public List<GameObject> objetos;
    private List<float> movimientosPiso;
    private int movimientoActual;
    
    public GameObject camaraOrbital;
    public GameObject camaraPrimeraPersona;

    public GameObject centro;
    public GameObject objetoQueEstoyMirando;
    public List<GameObject> pisoMovestible;
    public List<Light> luces;
    private bool bajando;

    public AudioSource fondo;
    public AudioSource lucesTitilando;


    public GameObject yo;

    public GameObject monstruo;
    private float movimientoMonstruo;
    public Light spotlight;
    public Light spotlightReflex;

    private float delaySpotLight=0;

    private int objetoActual;
    private float timeDelay;

    private float timeDelayRodri=0;
    private float timeDelayRodri2=0;
    private float random;

    private bool terminoEscenaInicial=false;
    private bool rotado=false;
    private float anguloDeRotacion;
    private float delayMovimiento=0;

    public GameObject particulasBaño;
    public GameObject particulasEntreBaño;


    void Start(){
        //camaraOrbital.transform.position = new Vector3(7,5,0);
        //camaraOrbital.transform.Rotate(new Vector3(30,-90,0))
        
        objetoActual = -1;
        objetoQueEstoyMirando=centro;
        camaraPrimeraPersona.GetComponent<Camera>().enabled = true;
        camaraPrimeraPersona.transform.position=new Vector3(25,5,8);
        camaraPrimeraPersona.transform.Rotate(new Vector3(0,-180,0));
        //camaraPrimeraPersona.transform.position=new Vector3(5,5,-4);
        //camaraPrimeraPersona.transform.Rotate(new Vector3(0,-90,0));
        camaraOrbital.GetComponent<Camera>().enabled = false;

        camaraOrbital.transform.Translate(new Vector3(5,5,5));
        camaraOrbital.transform.LookAt(centro.transform.position);
        
        //camaraOrbital.transform.Rotate(new Vector3(0,-30,0));
        timeDelay=0;

        bajando=false;
        if(pisoMovestible[0].transform.position.y <= 6f){
            bajando=true;
        }

        movimientosPiso=new List<float>();

        movimientosPiso.Add(0.0003f);
        movimientosPiso.Add(0.0002f);
        movimientosPiso.Add(0.0001f);
        movimientosPiso.Add(0.00009f);
        movimientosPiso.Add(0.00007f);
        movimientosPiso.Add(0.000008f);

        movimientoActual=0;

        fondo.loop=true;
        fondo.Play();

        anguloDeRotacion=0;
        
        foreach (GameObject piso in pisoMovestible){
            piso.active=false;
        }
    
    }   


    void Update()
    {    


        if(terminoEscenaInicial==true){
            if(Input.GetKeyDown(KeyCode.V))
                cambiarCamara();

            if(Input.GetKey(KeyCode.C) && timeDelay<=Time.time){
                objetoActual++;
                if(objetos.Count==objetoActual){
                    objetoActual = -1;
                    camaraOrbital.transform.Translate(new Vector3(centro.transform.position.x+1,centro.transform.position.y,centro.transform.position.z+1));
                    camaraOrbital.transform.LookAt(centro.transform.position);
                    objetoQueEstoyMirando=centro;
                    
                }
                else{
                    //camaraOrbital.Reset();
                    objetoQueEstoyMirando=objetos[objetoActual];
                    camaraOrbital.transform.Translate(new Vector3(objetos[objetoActual].transform.position.x+1,objetos[objetoActual].transform.position.y,objetos[objetoActual].transform.position.z+1));
                    camaraOrbital.transform.LookAt(objetos[objetoActual].transform.position);
                    //camaraOrbital.transform.position = new Vector3(objetos[0].transform.position.x,objetos[0].transform.position.y,objetos[0].transform.position.z);
                }
                
                timeDelay=Time.time+0.8f;
            }

            if(Input.GetKey(KeyCode.X) && timeDelay<=Time.time){
                objetoActual--;
                if(objetoActual<=-1){
                    objetoActual = -1;
                    camaraOrbital.transform.position=new Vector3(centro.transform.position.x+1,centro.transform.position.y,centro.transform.position.z+1);
                    camaraOrbital.transform.LookAt(centro.transform.position);
                    objetoQueEstoyMirando=centro;
                    
                }
                else{
                    //camaraOrbital.Reset();
                    objetoQueEstoyMirando=objetos[objetoActual];
                    camaraOrbital.transform.position=new Vector3(objetos[objetoActual].transform.position.x+1,objetos[objetoActual].transform.position.y,objetos[objetoActual].transform.position.z+1);
                    camaraOrbital.transform.LookAt(objetos[objetoActual].transform.position);
                    //camaraOrbital.transform.position = new Vector3(objetos[0].transform.position.x,objetos[0].transform.position.y,objetos[0].transform.position.z);
                }
                
                timeDelay=Time.time+0.8f;
            }

            if(Input.GetKey(KeyCode.R) ){
                camaraOrbital.transform.position=new Vector3(objetos[objetoActual].transform.position.x+1,objetos[objetoActual].transform.position.y,objetos[objetoActual].transform.position.z+1);
            }
            
            moverPiso();

            moverAvatarRodri();

            aparicionMonstruo();
        }else{
            movimientoInicial();
        }
    }

    void FixedUpdate()
    { 
        if(camaraPrimeraPersona.GetComponent<Camera>().enabled){
            if(Input.GetKey(KeyCode.LeftShift))
                moverAbajo();
            if(Input.GetKey(KeyCode.Space))
                moverArriba();
            if(Input.GetKey(KeyCode.W))
                desplazarseAdelante();
            if(Input.GetKey(KeyCode.S))
                desplazarseAtras();
            if(Input.GetKey(KeyCode.A))
                rotarIzquierda();
            if(Input.GetKey(KeyCode.D))
                rotarDerecha();
        }
        else{

        
            if(Input.GetKey(KeyCode.A)){
                OrbitalRotarIzquierda();
            }
            if(Input.GetKey(KeyCode.D)){
                OrbitalRotarDerecha();
            }
            if(Input.GetKey(KeyCode.W)){
                SubirCamaraOrbital();
            }
            if(Input.GetKey(KeyCode.S)){
                BajarCamaraOrbital();
            }/*
            if(Input.GetKey("up")){
                
            }
            if(Input.GetKey("down")){
                
            }*/
            if(Input.GetAxis("Mouse ScrollWheel")<0  || Input.GetKey("up"))
                Alejarse();
            if(Input.GetAxis("Mouse ScrollWheel")>0  || Input.GetKey("down"))
                Acercarse();
        }
    }

    private void moverAvatarRodri(){
        if( (timeDelayRodri2 <=Time.time) && (timeDelayRodri2<timeDelayRodri-6.4f) ){
                if(yo.active == false){
                    yo.active = true;
                    random=Random.Range(0,1);
                    if(random>=0.5f)
                        yo.transform.position=new Vector3(yo.transform.position.x+0.5f,yo.transform.position.y,yo.transform.position.z);
                    else   
                        yo.transform.position=new Vector3(yo.transform.position.x-0.5f,yo.transform.position.y,yo.transform.position.z);
                }else{
                    yo.active = false;
                    random=Random.Range(0,1);
                    if(random>=0.5f)
                        yo.transform.position=new Vector3(yo.transform.position.x-1,yo.transform.position.y,yo.transform.position.z);
                    else    
                        yo.transform.position=new Vector3(yo.transform.position.x+1,yo.transform.position.y,yo.transform.position.z);
                }

                timeDelayRodri2=Time.time+0.2f;
            }

            if(timeDelayRodri <=Time.time){
                if(yo.active == false){
                    yo.active = true;
                    yo.transform.position=new Vector3(10.28546f,0.005001068f,0.57f);
                }else{
                    yo.active = false;
                    yo.transform.position=new Vector3(10.28546f,0.005001068f,0.57f);
                }

                timeDelayRodri=Time.time+8;
                timeDelayRodri2=Time.time+0.5f;
            }
    }

    private void moverPiso(){
        if(monstruo.active==false){
            foreach (GameObject piso in pisoMovestible){

                //-0.04 hasta 0.03
                if(bajando==false){
                    piso.transform.position=new Vector3(piso.transform.position.x,piso.transform.position.y-movimientosPiso[movimientoActual],piso.transform.position.z);
                }else{
                    piso.transform.position=new Vector3(piso.transform.position.x,piso.transform.position.y+movimientosPiso[movimientoActual],piso.transform.position.z);;
                }
                
            } 

            if(pisoMovestible[0].transform.position.y<=0.01f  && pisoMovestible[0].transform.position.y>=-0.01f){
                movimientoActual=0;
            }else{
                if(pisoMovestible[0].transform.position.y>=0.01f || pisoMovestible[0].transform.position.y<=-0.015f){
                    movimientoActual=1;
                }else{
                    if(pisoMovestible[0].transform.position.y>=0.015f || pisoMovestible[0].transform.position.y<=-0.02f){
                        movimientoActual=2;
                    }else{
                        if(pisoMovestible[0].transform.position.y>=0.02f || pisoMovestible[0].transform.position.y<=-0.025f){
                            movimientoActual=3;
                        }else{
                            if(pisoMovestible[0].transform.position.y>=0.025f || pisoMovestible[0].transform.position.y<=-0.035f){
                                movimientoActual=4;
                            }else{
                                if(pisoMovestible[0].transform.position.y>=0.028f || pisoMovestible[0].transform.position.y<=-0.038f){
                                    movimientoActual=5;
                                }
                            }
                        }
                    }
                }
            }
            if(pisoMovestible[0].transform.position.y >= 0.04f){
                bajando=false;
            }else{
                if(pisoMovestible[0].transform.position.y <= -0.05f){
                    
                    bajando=true;
                }
            }
        }
    }

    private void aparicionMonstruo(){
        if(monstruo.active==true)
            monstruo.transform.position=new Vector3(monstruo.transform.position.x,monstruo.transform.position.y,monstruo.transform.position.z+0.01f);

        if(monstruo.transform.position.z>=-4.3f && monstruo.active==true){
            monstruo.active=false;
            particulasBaño.GetComponent<ParticleSystem>().Play();
            particulasEntreBaño.GetComponent<ParticleSystem>().Play();
            fondo.Stop();
            lucesTitilando.Play();
            foreach(Light luz in luces){
                luz.GetComponent<Light>().enabled = false;
            }
            foreach (GameObject piso in pisoMovestible){
                piso.active=true;
            }
        }

        if(monstruo.active==true && delaySpotLight<=Time.time){
            spotlight.GetComponent<Light>().enabled = !spotlight.GetComponent<Light>().enabled;
            spotlightReflex.GetComponent<Light>().enabled = !spotlight.GetComponent<Light>().enabled;
            spotlight.color=spotlight.color+Color.red/10.0f;
            spotlightReflex.color=spotlight.color+Color.red/10.0f;
            if(monstruo.transform.position.z>=-5f){
                delaySpotLight=Time.time+0.02f;
                foreach(Light luz in luces){
                    luz.color=luz.color+Color.red/10.0f;
                    luz.range+=1;
                }
                movimientoMonstruo=0.03f;
            }
            else{
                if(monstruo.transform.position.z>=-6f){
                    delaySpotLight=Time.time+0.3f;
                    movimientoMonstruo=0.04f;
                }
                else{
                    delaySpotLight=Time.time+0.8f;
                    movimientoMonstruo=0.08f;
                }
                
            }
            fondo.volume+=0.03f;
        }else{
            if(monstruo.active==false){
                spotlight.GetComponent<Light>().enabled = true;
                spotlightReflex.GetComponent<Light>().enabled = true;
                foreach(Light luz in luces){
                    luz.color=Color.white;
                    luz.range=10;
                    luz.GetComponent<Light>().enabled = true;
                }
                

            }
        }
    }


    private void movimientoInicial(){
        if(camaraPrimeraPersona.transform.position.z>-4){
                if(delayMovimiento<=Time.time){
                    desplazarseAdelante();
                    delayMovimiento=Time.time+0.02f;
                }
        }else{
            if(rotado==false){
                if(anguloDeRotacion<90){
                    camaraPrimeraPersona.transform.Rotate(new Vector3(0,0.5f,0));
                    anguloDeRotacion=anguloDeRotacion+0.5f;
                }else
                    rotado=true;
            }else{
                if(camaraPrimeraPersona.transform.position.x>5.3f){
                    if(delayMovimiento<=Time.time){
                        desplazarseAdelante();
                        delayMovimiento=Time.time+0.04f;
                    }
                }
                else{
                    if(camaraPrimeraPersona.transform.position.x>3){
                        if(delayMovimiento<=Time.time){
                            desplazarseAdelante();
                            delayMovimiento=Time.time+0.07f;
                            if(delaySpotLight<=Time.time){
                                spotlight.GetComponent<Light>().enabled = !spotlight.GetComponent<Light>().enabled;
                                spotlightReflex.GetComponent<Light>().enabled = !spotlight.GetComponent<Light>().enabled;
                                delaySpotLight=Time.time+1;
                                fondo.volume+=0.03f;
                            }
                        }
                    }else{
                        terminoEscenaInicial=true;
                    }
                }
            }
        }
    }

    private void Wait(){
     StartCoroutine(_wait());
    }
    IEnumerator _wait(){
        print(Time.time);
        yield return new WaitForSecondsRealtime(20000);
        print(Time.time);
    }

    void SubirCamaraOrbital(){
        camaraOrbital.transform.Translate(new Vector3(0,.15f,0));
        camaraOrbital.transform.LookAt(objetoQueEstoyMirando.transform.position);
    }
    void BajarCamaraOrbital(){
        camaraOrbital.transform.Translate(new Vector3(0,-.15f,0));
        camaraOrbital.transform.LookAt(objetoQueEstoyMirando.transform.position);
    }
    void AumentarZoom(){
        //camaraOrbital.transform.main.orthographicSize++;
        //camaraOrbital.GetComponent<Camera>().fieldOfView = Mathf.Lerp(camaraOrbital.GetComponent<Camera>().fieldOfView, 20, Time.deltaTime * 5);
        camaraOrbital.GetComponent<Camera>().fieldOfView -=3; 
    }
    void DisminuirZoom(){
        camaraOrbital.GetComponent<Camera>().fieldOfView +=3;
        
    }

    void OrbitalRotarIzquierda(){
        if(objetoActual == -1){
            camaraOrbital.transform.RotateAround (centro.transform.position,Vector3.up,20 * Time.deltaTime * 5);
        }
        else{
            camaraOrbital.transform.RotateAround (objetos[objetoActual].transform.position,Vector3.up,20 * Time.deltaTime * 5);
        }
    }

    private void Acercarse(){
       camaraOrbital.transform.Translate(new Vector3(0,0,1.5f));
    }
    private void Alejarse(){
        camaraOrbital.transform.Translate(new Vector3(0,0,-1.5f));
    }
    
    void OrbitalRotarDerecha(){
        
        if(objetoActual == -1){
            camaraOrbital.transform.RotateAround (centro.transform.position,Vector3.up,20 * Time.deltaTime * -5);
        }
        else{
            camaraOrbital.transform.RotateAround (objetos[objetoActual].transform.position,Vector3.up,20 * Time.deltaTime * -5);
        }
    }


    private void cambiarCamara(){
        camaraOrbital.GetComponent<Camera>().enabled = !camaraOrbital.GetComponent<Camera>().enabled;
        camaraPrimeraPersona.GetComponent<Camera>().enabled = !camaraPrimeraPersona.GetComponent<Camera>().enabled;

    }


    private void moverArriba(){
        camaraPrimeraPersona.transform.Translate(new Vector3(0,0.1f,0));
    }
    private void moverAbajo(){
        camaraPrimeraPersona.transform.Translate(new Vector3(0,-0.1f,0));
    }
    private void desplazarseAdelante(){
       camaraPrimeraPersona.transform.Translate(new Vector3(0,0,0.1f));
    }
    private void desplazarseAtras(){
        camaraPrimeraPersona.transform.Translate(new Vector3(0,0,-0.1f));
    }
    private void rotarIzquierda(){
        camaraPrimeraPersona.transform.Rotate(new Vector3(0,-2,0));
    }
    private void rotarDerecha(){
        camaraPrimeraPersona.transform.Rotate(new Vector3(0,2,0));
    }
}
