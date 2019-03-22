import {
  GET_MARKET_DATA,
  GET_CURRENCY_VALUES,
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
 [`${GET_CURRENCY_VALUES}_PENDING`] : (state, action) => {
    return {...state, currencyValuesFetching: false}
  },
  [`${GET_CURRENCY_VALUES}_FULFILLED`] : (state, action) => {
    return { ...state, currencyValuesFetching: false, currencyValues: action.payload }
  },
  [`${GET_CURRENCY_VALUES}_REJECTED`] : (state, action) => ({ ...state, currencyValuesFetching: false }),
}

// ------------------------------------
// Reducer
// ------------------------------------
const initialState = {
  marketDataFetching: false,
  marketData: [],
  currencyValuesFetching: false,
  currencyValues: {}
}

export default function authenticationReducer (state = initialState, action) {
  const handler = ACTION_HANDLERS[action.type];
  return handler ? handler(state, action) : state
}

