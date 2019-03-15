import {
  GET_ASSETS,
  UPDATE_PAPER_ASSETS,
} from '../actions/types';



const ACTION_HANDLERS = {
  [`${GET_ASSETS}_PENDING`] : (state, action) => {
    return {...state, paperAssetsFetching: true}
  },
  [`${GET_ASSETS}_FULFILLED`] : (state, action) => {
    return {...state, paperAssetsFetching: false, assets: action.payload}
  },
  [`${GET_ASSETS}_REJECTED`] : (state, action) => ({...state, paperAssetsFetching: false}),
 //==================================================== 
  [`${UPDATE_PAPER_ASSETS}_PENDING`] : (state, action) => {
    return {...state, paperAssetsFetching: true}
  },
  [`${UPDATE_PAPER_ASSETS}_FULFILLED`] : (state, action) => {
    return {...state, paperAssetsFetching: false}
  },
  [`${UPDATE_PAPER_ASSETS}_REJECTED`] : (state, action) => ({...state, paperAssetsFetching: false}),

}

const initialState = {
  paperAssetsFetching: false,
  assets: [],
}

export default function assetReducer (state = initialState, action) {
  const handler = ACTION_HANDLERS[action.type]
  return handler ? handler(state, action) : state
}
