import { createStore, applyMiddleware } from 'redux';
import thunk from 'redux-thunk';
import rootReducer from './reducers/rootReducer';
import { createLogger } from 'redux-logger';
import promiseMiddleware from 'redux-promise-middleware';

export default function configureStore(initialState={}) {
  return createStore(
    rootReducer,
    initialState,
    applyMiddleware(
      thunk,
      promiseMiddleware,
      createLogger()
    )
  );
}