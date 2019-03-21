import React from 'react';
import ReactDOM from 'react-dom';
import { Router } from 'react-router-dom';
import { Provider } from 'react-redux'
import history from './common/history';
import configureStore from './store';
import './index.css';
import App from './App';
import * as serviceWorker from './serviceWorker';

const env = process.env.NODE_ENV; //TODO: Create env. files
if(env === 'development'){
  document.masterApi = 'http://localhost:54850';
  document.marketApi = 'http://localhost:9999';
} else {
  document.masterApi = 'http://api.kryplproject.cz';
  document.marketApi = 'http://marketData.kryplproject.cz';
}

console.log(process.env.NODE_ENV);

ReactDOM.render(
  <Provider store={configureStore()}>
    <Router history={history}>
      <App />
    </Router>
  </Provider>,
  document.getElementById('root')
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: http://bit.ly/CRA-PWA
serviceWorker.unregister();
