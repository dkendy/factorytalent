import React, { useState } from 'react';
import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import axios from "axios";

import Keycloak from 'keycloak-js';

/*
  Init Options
*/
let initOptions = {
  url: import.meta.env.VITE_APP_KEYCLOAK_URL,
  realm: import.meta.env.VITE_APP_KEYCLOAK_REALM,
  clientId: import.meta.env.VITE_APP_KEYCLOAK_CLIENT_ID,
}

let kc = new Keycloak(initOptions);

kc.init({
  onLoad: 'login-required', // Supported values: 'check-sso' , 'login-required'
  checkLoginIframe: true,
  pkceMethod: 'S256'
}).then((auth) => {
  if (!auth) {
    window.location.reload();
  } else {
    /* Remove below logs if you are using this on production */
    console.info("Authenticated");
    console.log('auth', auth)
    console.log('Keycloak', kc)
    console.log('Access Token', kc.token)

    /* http client will use this header in every request it sends */
    axios.defaults.headers.common['Authorization'] = `Bearer ${kc.token}`;

    kc.onTokenExpired = () => {
      console.log('token expired')
    }
  }
}, () => {
  /* Notify the user if necessary */
  console.error("Authentication Failed");
});

function App() {


  const BACKEND_URL = import.meta.env.VITE_APP_BACKEND;

  const [infoMessage, setInfoMessage] = useState('');
  const [apiResult, setapiResult] = useState(null);
  const [loading, setLoading] = useState(false);
  const [initializing, setInitializing] = useState(false);


  /* To demonstrate : http client adds the access token to the Authorization header */
  const callBackend = () => {
    setInitializing(true);
    setLoading(true);
    axios
      .get(`${BACKEND_URL}/users/profile`)
      .then((response) => {
        setapiResult(response.data);
        setLoading(false); // Atualizar o estado de carregamento

      })
      .catch((err) => {
        setError("Error fetching profile data");
        setLoading(false); // Atualiza o estado de loading em caso de erro
        console.error(err);
      });

  };

  return (
    <div>
      <div class='col-md-12'>
        <h1>Factory Secured App</h1>
      </div>

      <div class='col-md-12'>

        <ul class="nav">
         
          <li class="nav-item">
            <a class="nav-link" onClick={() => { setInfoMessage(kc.token) }} href="#">Show Access Token</a>
          </li>
          <li class="nav-item">
            <a class="nav-link" onClick={callBackend} href="#">Who am I?</a>
          </li>
          <li class="nav-item">
            <a class="nav-link" href="#" onClick={() => { kc.logout({ redirectUri: 'http://localhost:3000/' }) }} >Log out</a>
          </li>
        </ul>

      </div>


     
      <div class='col-md-12'>
        <div class="card">
          <div class="card-body">
            <p style={{ wordBreak: 'break-all' }} id='infoPanel'>
              {infoMessage}
            </p>

            {initializing ?
            (
              loading ?
                (
                  <p>Carregando...</p>
                ) : (
                  <p>
                    <span>Nome: {apiResult?.firstName}</span>
                    <br />
                    <span>Email: {apiResult?.email}</span>
                  </p>)
            ) : <span />
          }



          </div>
        </div>
      </div>
    </div>

  );
}


export default App;