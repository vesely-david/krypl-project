import React from 'react'
import { connect } from 'react-redux'

const PaperContainer = () => {
  return (
    <h1>Paper</h1>
  )
}

const mapDispatchToProps = {
}

const mapStateToProps = (state) => ({
})

export default connect(mapStateToProps, mapDispatchToProps)(PaperContainer)
