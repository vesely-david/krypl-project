import {
  GET_MARKET_DATA,
} from '../actions/types';


const ACTION_HANDLERS = {
  [`${GET_MARKET_DATA}_PENDING`] : (state, action) => {
    return {...state, marketDataFetching: false}
  },
  [`${GET_MARKET_DATA}_FULFILLED`] : (state, action) => {
    return { ...state, marketDataFetching: false, marketData: action.payload }
  },
  [`${GET_MARKET_DATA}_REJECTED`] : (state, action) => ({ ...state, marketDataFetching: false }),
 //====================================================   
}

// ------------------------------------
// Reducer
// ------------------------------------
const initialState = {
  marketDataFetching: false,
  marketData: []
}

export default function authenticationReducer (state = initialState, action) {
  const handler = ACTION_HANDLERS[action.type];
  return handler ? handler(state, action) : state
}

