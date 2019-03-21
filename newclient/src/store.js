import { createStore, applyMiddleware } from 'redux';
import thunk from 'redux-thunk';
import rootReducer from './reducers/rootReducer';
import { createLogger } from 'redux-logger';
import promiseMiddleware from 'redux-promise-middleware';
import { setupInterceptors } from './middleware/axiosMiddleware';


export default function configureStore(initialState={}) {
  const store = createStore(
    rootReducer,
    initialState,
    applyMiddleware(
      thunk,
      promiseMiddleware,
      createLogger()
    )
  );
  setupInterceptors(store); //Axios middleware
  return store;
}