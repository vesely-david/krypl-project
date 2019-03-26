import { combineReducers } from 'redux';
import userReducer from './userReducer';
import assetReducer from './assetReducer';
import marketDataReducer from './marketDataReducer';
import paperReducer from './paperReducer';
import realReducer from './realReducer';
import strategyReducer from './strategyReducer';


export default combineReducers({
  user: userReducer,
  assets: assetReducer,
  strategies: strategyReducer,
  marketData: marketDataReducer,
  paper: paperReducer,
  real: realReducer,
});