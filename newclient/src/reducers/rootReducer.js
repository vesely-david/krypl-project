import { combineReducers } from 'redux';
import userReducer from './userReducer';
import assetReducer from './assetReducer';
import marketDataReducer from './marketDataReducer';
import paperReducer from './paperReducer';


export default combineReducers({
  user: userReducer,
  assets: assetReducer,
  marketData: marketDataReducer,
  paper: paperReducer,
});