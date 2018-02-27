//import { injectReducer } from '../../store/reducers'

export default (store) => ({
  path : 'real',
  /*  Async getComponent is only invoked when route matches   */
  getComponent (nextState, cb) {
    /*  Webpack - use 'require.ensure' to create a split point
        and embed an async module loader (jsonp) when bundling   */
    require.ensure([], (require) => {
      /*  Webpack - use require callback to define
          dependencies for bundling   */
      const Real = require('./containers/RealContainer').default
   //   const reducer = require('./modules/real').default

      /*  Add the reducer to the store on key 'real'  */
   //   injectReducer(store, { key: 'real', reducer })

      /*  Return getComponent   */
      cb(null, Real)

    /* Webpack named bundle   */
    }, 'real')
  }
})
