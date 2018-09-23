import { injectReducer } from '../../store/reducers'

export default (store) => ({
  path : 'backtest',
  /*  Async getComponent is only invoked when route matches   */
  getComponent (nextState, cb) {
    /*  Webpack - use 'require.ensure' to create a split point
        and embed an async module loader (jsonp) when bundling   */
    require.ensure([], (require) => {
      /*  Webpack - use require callback to define
          dependencies for bundling   */
      const Backtest = require('./containers/BacktestContainer').default
      //const reducer = require('./modules/backtest').default

      /*  Add the reducer to the store on key 'backtest'  */
      //injectReducer(store, { key: 'backtest', reducer })

      /*  Return getComponent   */
      cb(null, Backtest)

    /* Webpack named bundle   */
    }, 'backtest')
  }
})
