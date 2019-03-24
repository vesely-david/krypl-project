import axios from 'axios';

async function login(credentials){
  const response = await axios.post(
    `${document.masterApi}/user/login`, 
    JSON.stringify(credentials), {
    headers: {
      'Accept': 'application/json',
      'Content-type': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    },
  })
  return response.data;
}

async function register(credentials){
  const response = await axios.post(
    `${document.masterApi}/user/register`, 
    JSON.stringify(credentials), {
    headers: {
      'Accept': 'application/json',
      'Content-type': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    },
  })
  return response.data;  
}

async function getApiKeys(){
  const response = await axios.get(
    `${document.masterApi}/user/apikeys`, {
    headers: {
      'Accept': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    },
  })
  return response.data;  
}

async function deleteApiKey(apiKeyId){
  const response = await axios.delete(
    `${document.masterApi}/user/apikeys/${apiKeyId}`, {
    headers: {
      'Accept': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    },
  })
  return response.data;  
}

async function editApiKey(apiKey){
  const response = await axios.post(
    `${document.masterApi}/user/apikeys`, 
    JSON.stringify(apiKey), {
    headers: {
      'Accept': 'application/json',
      'Content-type': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    },
  })
  return response.data;  
}

export const userService = {
  login,
  register,
  getApiKeys,
  deleteApiKey,
  editApiKey,
}
