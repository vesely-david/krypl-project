import {
  get,
  post
} from '../common/fetchOptions';

async function login(credentials){
  const response = await fetch(`${document.masterApi}/user/login`, {...post(), body: JSON.stringify(credentials)});

  if (response.ok) {
    return response.json();
  } else{
    const json = await response.json();
    throw new Error(json.message);
  }
}

export const userService = {
  login,
}