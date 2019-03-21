import axios from 'axios';
import { 
  LOGOUT,
} from '../actions/types';

export const setupInterceptors =  (store) => {
  axios.interceptors.response.use(function (response) {
    const token = response.headers['refresh-auth-token'];
      if(token) localStorage.setItem('user_token', token);
      return response;
  }, function (error) {
    if ( !error.response || error.response.status === 500) {
      return Promise.reject('Internal server error - contact support');
    }

    if ( error.response.status === 401) {
      localStorage.clear();
      store.dispatch({ type: LOGOUT });
      return Promise.reject('Your session has expired. Please log in again.');
    }
    console.error(error.response.data);
    return Promise.reject(error.response.data);
  });
};