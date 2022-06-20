import { _delete, get, post, put } from '.'

export async function createRace({
  ageThresholds,
  attributes,
  attributesText,
  ageText,
  description,
  extraAttributes,
  extraLanguages,
  languageIds,
  languagesText,
  name,
  names,
  namesText,
  parentId,
  peopleText,
  size,
  sizeText,
  speedText,
  speeds,
  statureRoll,
  traits,
  weightRolls,
  weightText
}) {
  return await post('/races', {
    ageThresholds,
    attributes,
    attributesText,
    ageText,
    description,
    extraAttributes,
    extraLanguages,
    languageIds,
    languagesText,
    name,
    names,
    namesText,
    parentId,
    peopleText,
    size,
    sizeText,
    speedText,
    speeds,
    statureRoll,
    traits,
    weightRolls,
    weightText
  })
}

export async function deleteRace(id) {
  return await _delete(`/races/${id}`)
}

export async function getPeople(id) {
  return await get(`/races/${id}/people`)
}

export async function getRace(id) {
  return await get(`/races/${id}`)
}

export async function getRaces({ deleted, search, sort, desc, index, count }) {
  const query = Object.entries({ deleted, search, sort, desc, index, count })
    .filter(([, value]) => typeof value !== 'undefined' && value !== null)
    .map(pair => pair.join('='))
    .join('&')
  return await get(`/races?${query}`)
}

export async function updateRace(
  id,
  {
    ageThresholds,
    attributes,
    attributesText,
    ageText,
    description,
    extraAttributes,
    extraLanguages,
    languageIds,
    languagesText,
    name,
    names,
    namesText,
    peopleText,
    size,
    sizeText,
    speedText,
    speeds,
    statureRoll,
    traits,
    weightRolls,
    weightText
  }
) {
  return await put(`/races/${id}`, {
    ageThresholds,
    attributes,
    attributesText,
    ageText,
    description,
    extraAttributes,
    extraLanguages,
    languageIds,
    languagesText,
    name,
    names,
    namesText,
    peopleText,
    size,
    sizeText,
    speedText,
    speeds,
    statureRoll,
    traits,
    weightRolls,
    weightText
  })
}
